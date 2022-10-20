Imports System.IO
Imports System.Data.SqlClient
Public Class WfrmCronologiaDettaglioSede
    Inherits System.Web.UI.Page
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub
    Sub CaricaGriglia()
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsUtilizzoSede As DataSet = New DataSet
        MyCommand = New SqlClient.SqlDataAdapter("SP_RITORNACRONOLOGIASEDI", CType(Session("conn"), SqlClient.SqlConnection))
        MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure
        MyCommand.SelectCommand.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = Request.QueryString("id")
        MyCommand.Fill(DsUtilizzoSede)
        'dtgSede.CurrentPageIndex = 0
        dtgSede.DataSource = DsUtilizzoSede
        'controllo se ci sono dei record
        If DsUtilizzoSede.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader

            Session("RisultatoGriglia") = DsUtilizzoSede
            dtgSede.Caption = "Cronologia Stato Sedi"
        Else
            dtgSede.Caption = " Nessuna cronologia registrata."

        End If
        dtgSede.DataBind()


        ''blocco per la creazione della datatable per la stampa 

        ''nome e posizione di lettura delle colopnne a base 0
        'Dim NomeColonne(DsUtilizzoSede.Tables(0).Columns.Count) As String
        'Dim NomiCampiColonne(DsUtilizzoSede.Tables(0).Columns.Count) As String
        'Dim intX As Integer
        'For intX = 0 To DsUtilizzoSede.Tables(0).Columns.Count - 1
        '    NomeColonne(intX) = DsUtilizzoSede.Tables(0).Columns(intX).ColumnName
        '    NomiCampiColonne(intX) = DsUtilizzoSede.Tables(0).Columns(intX).ColumnName
        'Next
        'CaricaDataTablePerStampa(DsUtilizzoSede, DsUtilizzoSede.Tables(0).Columns.Count - 1, NomeColonne, NomiCampiColonne)

    End Sub
    Private Sub dtgUtilizzoSede_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSede.PageIndexChanged
        '        dtgSede.CurrentPageIndex = e.NewPageIndex
        dtgSede.DataSource = Session("RisultatoGriglia")
        dtgSede.DataBind()
        dtgSede.SelectedIndex = -1
    End Sub


End Class