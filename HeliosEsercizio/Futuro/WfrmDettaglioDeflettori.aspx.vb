Imports System.Data.SqlClient
Public Class WfrmDettaglioDeflettori
    Inherits System.Web.UI.Page
    Public Property Sp_Name() As String
        Get
            Return Session("Sp_Name")
        End Get
        Set(ByVal Value As String)
            Session("Sp_Name") = Value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myDataTable As DataTable
        Select Case Sp_Name
            Case "SP_DEFLETTORI_DET_SANZIONI_LIEVI"
                myDataTable = SP_DEFLETTORI_DET_SANZIONI_LIEVI(Session("idprogetto"))
            Case "SP_DEFLETTORI_DET_SANZIONI_GRAVI"
                myDataTable = SP_DEFLETTORI_DET_SANZIONI_GRAVI(Session("idprogetto"))
            Case "SP_DEFLETTORI_DET_ISPEZIONI"
                myDataTable = SP_DEFLETTORI_DET_ISPEZIONI(Session("idprogetto"))
        End Select

        If Not IsNothing(myDataTable) Then
            dtDeflettori.DataSource = myDataTable

            'System.Web.UI.WebControls.Unit.Pixel(10)

            dtDeflettori.DataBind()
        Else

            lblmessaggiosopra.Visible = True

        End If
        If Not IsNothing(Session("Title")) Then
            lblTitle.Text = Session("Title")
        End If
    End Sub
    Private Function SP_DEFLETTORI_DET_ISPEZIONI(ByVal idprogetto As Integer) As DataTable


        ' Create a SqlParameter for each parameter in the stored procedure.
        Dim IdAttivita As New SqlParameter("@IdAttivita", idprogetto)
        Dim Output As New SqlParameter("@TotIspezioni ", 0)
        Output.Direction = ParameterDirection.Output

        Dim GridViewSorce As New DataSet
        Dim sqlCMD As New SqlCommand
        Try
            sqlCMD.Connection = (Session("conn"))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.CommandText = Sp_Name
            sqlCMD.Parameters.Add(IdAttivita)
            sqlCMD.Parameters.Add(Output)
            Dim adapter As New SqlDataAdapter(sqlCMD)
            adapter.Fill(GridViewSorce)
        Catch ex As Exception

        End Try
        If GridViewSorce.Tables.Count > 0 Then
            Return GridViewSorce.Tables(0)
        Else
            Return Nothing
        End If



    End Function
    Private Function SP_DEFLETTORI_DET_SANZIONI_LIEVI(ByVal idprogetto As Integer) As DataTable

        Dim GridViewSorce As New DataSet

        Dim IdAttivita As New SqlParameter("@IdAttivita", idprogetto)
        Dim _PercentualeSuTotale As New SqlParameter("@PercentualeSuTotale", 0.0)
        _PercentualeSuTotale.Direction = ParameterDirection.Output
        Dim _Deflettore As New SqlParameter("@Deflettore", 0.0)
        _Deflettore.Direction = ParameterDirection.Output
        Dim _TotSanzioniLievi As New SqlParameter("@TotSanzioniLievi", 0)
        _TotSanzioniLievi.Direction = ParameterDirection.Output

        Dim sqlCMD As New SqlCommand
        Try
            sqlCMD.Connection = (Session("conn"))

            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.CommandText = "SP_DEFLETTORI_DET_SANZIONI_LIEVI"
            sqlCMD.Parameters.Add(IdAttivita)
            sqlCMD.Parameters.Add(_PercentualeSuTotale)
            sqlCMD.Parameters.Add(_Deflettore)
            sqlCMD.Parameters.Add(_TotSanzioniLievi)

            Dim adapter As New SqlDataAdapter(sqlCMD)
            adapter.Fill(GridViewSorce)
        Catch ex As Exception

        End Try
        If GridViewSorce.Tables.Count > 0 Then
            Return GridViewSorce.Tables(0)
        Else
            Return Nothing
        End If


    End Function
    Private Function SP_DEFLETTORI_DET_SANZIONI_GRAVI(ByVal idprogetto As Integer) As DataTable

        Dim GridViewSorce As New DataSet

        Dim IdAttivita As New SqlParameter("@IdAttivita", idprogetto)
        Dim _PercentualeSuTotale As New SqlParameter("@PercentualeSuTotale", 0.0)
        _PercentualeSuTotale.Direction = ParameterDirection.Output
        Dim _Deflettore As New SqlParameter("@Deflettore", 0.0)
        _Deflettore.Direction = ParameterDirection.Output
        Dim _TotSanzioniGravi As New SqlParameter("@TotSanzioniGravi", 0)
        _TotSanzioniGravi.Direction = ParameterDirection.Output

        Dim sqlCMD As New SqlCommand
        Try
            sqlCMD.Connection = (Session("conn"))

            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.CommandText = "SP_DEFLETTORI_DET_SANZIONI_GRAVI"
            sqlCMD.Parameters.Add(IdAttivita)
            sqlCMD.Parameters.Add(_PercentualeSuTotale)
            sqlCMD.Parameters.Add(_Deflettore)
            sqlCMD.Parameters.Add(_TotSanzioniGravi)
            Dim adapter As New SqlDataAdapter(sqlCMD)
            adapter.Fill(GridViewSorce)
        Catch ex As Exception

        End Try
        If GridViewSorce.Tables.Count > 0 Then
            Return GridViewSorce.Tables(0)
        Else
            Return Nothing
        End If


    End Function
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click

    End Sub
End Class