﻿Public Class WfrmInfoPrefissiDocumentiRimborsi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim sqlDataSet As DataSet

        strSql = " SELECT IDPrefisso, Prefisso, TipologiaDocumento, ModalitàInvio FROM PrefissiEntitàDocumenti Where TipoInserimento=2 ORDER BY ORDINE"
        sqlDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgElencoPrefissi.DataSource = sqlDataSet
        dgElencoPrefissi.DataBind()
    End Sub

End Class