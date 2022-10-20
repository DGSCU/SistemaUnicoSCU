Imports System.Data.SqlClient

Public Class WfrmEditorElencoModelli
    Inherits System.Web.UI.Page
    Public strsql As String
    Public dtrgenerico As SqlDataReader
    Public dtsRisRicerca As DataSet

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
        If Page.IsPostBack = False Then
            ddlAreaModello.Items.Clear()
            strsql = "select IdArea,Area from Editor_Aree"
            ddlAreaModello.DataSource = MakeParentTable(strsql)
            ddlAreaModello.DataTextField = "ParentItem"
            ddlAreaModello.DataValueField = "id"
            ddlAreaModello.DataBind()
        Else

        End If
        If Request.QueryString("vengoda") = "file" Then
            dtgRisultatoRicerca.SelectedIndex = -1
            dtgRisultatoRicerca.DataSource = Session("dtRisultatoModelli")
            dtgRisultatoRicerca.DataBind()
        End If
    End Sub

    Private Function MakeParentTable(ByVal strquery As String) As DataSet

        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "Id"
        myDataColumn.Caption = "Id"
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
        ChiudiDataReader(dtrgenerico)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
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

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        dtgRisultatoRicerca.CurrentPageIndex = 0
        CaricaGriglia()
    End Sub
    Private Sub CaricaGriglia()
        strsql = "select UserName,IdRegioneCompetenza,RegCompe,IdArea,IdModello,Area,IdUtenteArea,NomeLogico,NomeFisico,Path,UsernameProprietario,dbo.FormatoData(DataCreazione) as DataCreazione ,Descrizione from VW_Editor_ElencoModelli_1 where username = '" & Session("Utente") & "'"
        If Trim(ddlAreaModello.SelectedItem.Text) <> "Selezionare" Then
            strsql = strsql & " and Idarea = '" & Replace(ddlAreaModello.SelectedValue, "'", "''") & "'"
        Else

        End If
        If Trim(TxtNomeLogico.Text) <> "" Then
            strsql = strsql & " and NomeLogico like '%" & Replace(TxtNomeLogico.Text, "'", "''") & "%'"
        End If
        If Trim(txtDescrizione.Text) <> "" Then
            strsql = strsql & " and Descrizione like '%" & Replace(txtDescrizione.Text, "'", "''") & "%'"
        End If

        dtsRisRicerca = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dtgRisultatoRicerca.DataSource = dtsRisRicerca
        Session("dtRisultatoModelli") = dtgRisultatoRicerca.DataSource
        dtgRisultatoRicerca.SelectedIndex = -1
        dtgRisultatoRicerca.CurrentPageIndex = 0
        dtgRisultatoRicerca.DataBind()
        If (dtgRisultatoRicerca.Items.Count = 0) Then
            dtgRisultatoRicerca.Caption = "La ricerca non ha prodotto risultati"
        Else
            dtgRisultatoRicerca.Caption = "Risultato Ricerca Modelli"
        End If
        dtsRisRicerca = Nothing
    End Sub

    Private Sub dtgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        ScaricaFile(dtgRisultatoRicerca.SelectedItem.Cells(9).Text, dtgRisultatoRicerca.SelectedItem.Cells(8).Text, dtgRisultatoRicerca.SelectedItem.Cells(7).Text, dtgRisultatoRicerca.SelectedItem.Cells(6).Text, dtgRisultatoRicerca.SelectedItem.Cells(10).Text, dtgRisultatoRicerca.SelectedItem.Cells(1).Text, dtgRisultatoRicerca.SelectedItem.Cells(2).Text, dtgRisultatoRicerca.SelectedItem.Cells(3).Text, dtgRisultatoRicerca.SelectedItem.Cells(4).Text)
    End Sub
    Private Sub ScaricaFile(ByVal path, ByVal nomedoc, ByVal model, ByVal areaid, ByVal regioncompe, ByVal nomelogico, ByVal descrizione, ByVal data, ByVal username)
        Dim strPercorsoFile As String
        Dim strNomeFile As String
        Dim idmodello As String
        Dim IdArea As String
        Dim RegioneCompeten As String
        strPercorsoFile = path
        strNomeFile = nomedoc
        idmodello = model
        IdArea = areaid
        RegioneCompeten = regioncompe
        Response.Redirect("WfrmEditorModelliDownload.aspx?model=" & idmodello)

    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.SelectedIndex = -1
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("dtRisultatoModelli")
        dtgRisultatoRicerca.DataBind()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

End Class