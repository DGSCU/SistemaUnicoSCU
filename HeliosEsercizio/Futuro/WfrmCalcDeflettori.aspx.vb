Imports System.Data.SqlClient

Public Class WfrmCalcDeflettori
    Inherits System.Web.UI.Page

    Dim anno As String
    Dim deflettoriAnno As String
    Dim totIspezioni As Integer
    Dim totSanzioniGravi As Integer
    Dim totSanzioniLievi As Integer
    Dim percentualeSuTotale As Double
    Dim deflettore As Integer
    Dim percentualeSuTotaleGr As Double
    Dim deflettoreGr As Integer
    Dim redirectPaginaDettaglio As String = "<script> myWin = window.open ('WfrmDettaglioDeflettori.aspx','','width=400,height=400,toolbar=no,location=no,resizable=yes,menubar=no,scrollbars=yes')</script>"

#Region "Utility"


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
        EsecuzioneStoredProcedures()
        '====================SANZIONI LIEVI==========================
        lblDiffLieve.Text = anno
        lblIspAnno.Text = anno

        txtTotIspAnnoLIEVE.Text = totIspezioni
        txtTotDiffLieve.Text = totSanzioniLievi
        lblValorelieveperc.Text = String.Format("{0:N2}", percentualeSuTotale)
        lblValoreLieveAnno.Text = deflettore

        '====================SANZIONI GRAVI==========================
        txtTotIspAnnoGrave.Text = totIspezioni
        txtTotSanzioneAnnoGRAVE.Text = totSanzioniGravi
        lblIspAnnoGrave.Text = anno
        lblSazioniAnno.Text = anno
        lblValoreGravePerc.Text = String.Format("{0:N2}", percentualeSuTotaleGr)


        lblValoreGraveAnno.Text = deflettoreGr
        txtTotDeffSanzioni.Text = deflettore + deflettoreGr



    End Sub

    Private Function CallStored(ByVal Sp_Name As String, ByVal Inputparameter As String, ByVal outParameterName As String) As String



        ' Create a SqlParameter for each parameter in the stored procedure.
        Dim IdAttivita As New SqlParameter("@IdAttivita", CInt(Inputparameter))
        Dim Output As New SqlParameter(outParameterName, 0)
        Output.Direction = ParameterDirection.Output


        Dim sqlCMD As New SqlCommand
        Try
            sqlCMD.Connection = (Session("conn"))

            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.CommandText = Sp_Name
            sqlCMD.Parameters.Add(IdAttivita)
            sqlCMD.Parameters.Add(Output)
            sqlCMD.ExecuteNonQuery()
            sqlCMD.Dispose()
        Catch ex As Exception

        End Try

        Return Output.Value

    End Function
    '
    Private Sub EsecuzioneStoredProcedures()

        anno = CallStored("SP_DEFLETTORI_ANNO", Session("idprogetto"), "@Anno")
        totIspezioni = CallStored("SP_DEFLETTORI_DET_ISPEZIONI", Session("idprogetto"), "@TotIspezioni")
        totSanzioniGravi = CallStored("SP_DEFLETTORI_DET_SANZIONI_GRAVI", Session("idprogetto"), "@TotSanzioniGravi")
        If anno = "" Then
            anno = 0
        End If
        DettaglioSanzioniLievi(Session("idprogetto"))
        DettaglioSanzioniGravi(Session("idprogetto"))

    End Sub

    Private Function DettaglioSanzioniLievi(ByVal idprogetto As Integer) As Boolean


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

            sqlCMD.ExecuteNonQuery()
            sqlCMD.Dispose()
        Catch ex As Exception
            Return False
        End Try

        percentualeSuTotale = _PercentualeSuTotale.Value
        deflettore = _Deflettore.Value
        totSanzioniLievi = _TotSanzioniLievi.Value

        Return True




    End Function
    Private Function DettaglioSanzioniGravi(ByVal idprogetto As Integer) As Boolean


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

            sqlCMD.ExecuteNonQuery()
            sqlCMD.Dispose()
        Catch ex As Exception
            Return False
        End Try

        percentualeSuTotaleGr = _PercentualeSuTotale.Value
        deflettoreGr = _Deflettore.Value
        totSanzioniGravi = _TotSanzioniGravi.Value

        Return True




    End Function


    Private Sub lTotIspAnnoGrave_Click(ByVal sender As System.Object, ByVal e As ImageClickEventArgs) Handles lTotIspAnnoGrave.Click
        Session("Sp_Name") = "SP_DEFLETTORI_DET_ISPEZIONI"
        Session("Title") = "Verifiche " & anno
        Response.Write(redirectPaginaDettaglio)
    End Sub


    Private Sub lTotIspAnno_Click(ByVal sender As System.Object, ByVal e As ImageClickEventArgs) Handles lTotIspAnno.Click
        Session("Sp_Name") = "SP_DEFLETTORI_DET_ISPEZIONI"
        Session("Title") = "Verifiche " & anno
        Response.Write(redirectPaginaDettaglio)
    End Sub

    Private Sub lTotDiffLieve_Click(ByVal sender As System.Object, ByVal e As ImageClickEventArgs) Handles lTotDiffLieve.Click

        Session("Sp_Name") = "SP_DEFLETTORI_DET_SANZIONI_LIEVI"
        Session("Title") = "Sanzioni Lievi"
        Response.Write(redirectPaginaDettaglio)

    End Sub
    Private Sub lTotSanzioneAnnoGR_Click(ByVal sender As System.Object, ByVal e As ImageClickEventArgs) Handles lTotSanzioneAnnoGR.Click
        Session("Sp_Name") = "SP_DEFLETTORI_DET_SANZIONI_GRAVI"
        Session("Title") = "Sanzioni Gravi"
        Response.Write(redirectPaginaDettaglio)
    End Sub
End Class