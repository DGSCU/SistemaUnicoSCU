Public Class WfrmDocFileDownload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim IdDocumento As Integer
        Dim strEsito As String
        Dim strOrigine As String = Request.QueryString("Origine")

        Select Case strOrigine
            Case "Volontario"
                IdDocumento = Request.QueryString("IdDocumentoEntità")
                strEsito = clsGestioneDocumenti.RecuperaDocumentoVolontario(IdDocumento, Session("Utente"), Session("conn"))
                Response.Redirect(strEsito)
            Case "Sistemi"
                IdDocumento = Request.QueryString("IdEnteDocumento")
                strEsito = clsGestioneDocumentiAccreditamento.RecuperaDocumentoSistemi(IdDocumento, Session("Utente"), Session("conn"))
                Response.Redirect(strEsito)
        End Select

    End Sub

End Class