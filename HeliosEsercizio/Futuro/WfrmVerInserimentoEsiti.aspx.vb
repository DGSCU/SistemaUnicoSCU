Imports System.Data.SqlClient
Public Class WfrmVerInserimentoEsiti
    Inherits System.Web.UI.Page
    Shared strID As String

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    'Protected WithEvents LblDescrizione As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDescrizione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lblmsg As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents LblNote As System.Web.UI.WebControls.Label
    'Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    

    Sub InsertEsito()
        'insert nella tabella TVerificheEsitoTemplate
        Dim cmdIns As SqlCommand
        Dim strSql As String
        Dim dtrPrendiId As SqlClient.SqlDataReader
        Dim IntIdUser As Integer
        Try
            strSql = " INSERT INTO  TVerificheEsitoTemplate ( Esito, Abilitato,DataInserimento, UserInseritore ,note) " & _
                     " VALUES ('" & txtDescrizione.Text.Replace("'", "''") & "' ,1, getdate(),'" & Session("Utente") & " ','" & txtNote.Text.Replace("'", "''") & "')"
            cmdIns = ClsServer.EseguiSqlClient(strSql, Session("conn"))
            lblmsg.Visible = True
            lblmsg.Text = "Inserimento eseguito con successo."

            strID = RecuperaID()

        Catch ex As Exception
            lblmsg.Visible = True
            lblmsg.Text = "Errore imprevisto.Contattare l'assistenza."
        End Try
    End Sub
    Private Function RecuperaID() As Integer
        '---recupero l'id appena inserito in TVerificheEsitoTemplate
        Dim strSql As String
        Dim dtrEsito As SqlDataReader
        Dim strID As String
        strSql = " select SCOPE_IDENTITY() Id"
        dtrEsito = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrEsito.Read()
        strID = dtrEsito("Id")
        dtrEsito.Close()
        dtrEsito = Nothing
        Return strID
    End Function

    Sub CloseForm()
        If Request.QueryString("VengoDa") = "Requisito" Then
            Response.Redirect("WfrmGestioneRequisiti.aspx?TipoAzione=" + Request.QueryString("TipoAzione") + "&IdEsito=" & strID & "&IDVersioneVerifiche=" + Request.QueryString("IDVersioneVerifiche") + "&IdRequisito=" + Request.QueryString("IdRequisito") + "&VengoDa=Requisito&StatoVersione=" & Request.QueryString("StatoVersione") & "")
        Else
            Response.Redirect("WfrmVerGestioneEsiti.aspx?VengoDa=Esiti")
        End If
    End Sub
    Protected Sub cmdSalva_Click1(sender As Object, e As EventArgs) Handles cmdSalva.Click
        InsertEsito()
        CloseForm()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        CloseForm()
    End Sub

    Protected Sub txtDescrizione_TextChanged(sender As Object, e As EventArgs) Handles txtDescrizione.TextChanged

    End Sub
End Class