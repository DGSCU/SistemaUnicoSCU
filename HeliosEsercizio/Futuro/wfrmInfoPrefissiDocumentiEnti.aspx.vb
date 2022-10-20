Imports System.Data.SqlClient

Public Class wfrmInfoPrefissiDocumentiEnti
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
      VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub
    Sub CaricaGriglia()
        Dim strSql As String
        Dim sqlDataSet As DataSet

        Dim AlboEnte As String
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

        strSql = " SELECT IDPrefisso, Prefisso, TipologiaDocumento, ModalitàInvio FROM PrefissiEntiDocumenti WHERE "
        If AlboEnte = "" Then
            strSql &= "  (Albo = 'SCU' OR Albo is null)"
        Else
            strSql &= "  (Albo = '" & AlboEnte & "' OR Albo is null)"
        End If
        strSql &= " and prefisso not in ('ATTOCOSTITUTIVO_', 'STATUTO_', 'DELIBERA_', 'CARTAIMPEGNOETICO_') "
        strSql &= "and not prefisso like 'CV%' "
        strSql = strSql & " ORDER BY ORDINE"
        sqlDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgElencoPrefissi.DataSource = sqlDataSet
        dgElencoPrefissi.DataBind()

    End Sub

End Class