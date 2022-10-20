Imports System.Data.SqlClient

Public Class WfrmProgrammiInfoPrefissi
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
        Dim strSql As String
        Dim sqlDataSet As DataSet
        If Session("Sistema") = "Helios" Then
            strSql = " SELECT a.IDPrefisso, a.Prefisso, a.TipologiaDocumento, a.ModalitàInvio FROM PrefissiProgrammiDocumenti a where a.prefisso <> 'PROGGG_' ORDER BY a.ORDINE"
        Else
            strSql = " SELECT a.IDPrefisso, a.Prefisso, a.TipologiaDocumento, replace(a.ModalitàInvio,'HELIOS','FUTURO') AS ModalitàInvio FROM PrefissiProgrammiDocumenti a where a.prefisso <> 'PROG_' ORDER BY a.ORDINE"
        End If
        sqlDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgElencoPrefissi.DataSource = sqlDataSet
        dgElencoPrefissi.DataBind()
    End Sub

End Class