Public Class ver_visibilita
    Inherits System.Web.UI.Page
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim dtrgenerico As SqlClient.SqlDataReader
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Dim strsql As String
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents imgConferma As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents TxtPeriodo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtNote As System.Web.UI.WebControls.TextBox

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If IsPostBack = False Then
            strsql = "SELECT * " & _
                       "FROM TVerificheVisibilita "
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                TxtPeriodo.Text = dtrgenerico("Periodo")
                TxtNote.Text = dtrgenerico("Note")
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
        End If
        End If
    End Sub

    
   
    Private Sub imgConferma_Click(sender As Object, e As EventArgs) Handles imgConferma.Click

        
        If (IsNumeric(TxtPeriodo.Text)) Then


            strsql = "UPDATE TVerificheVisibilita SET " & _
            "Periodo = '" & Replace(TxtPeriodo.Text, "'", "''") & "', " & _
            "DataUltimaModifica =  getdate(), " & _
            "Note = '" & Replace(TxtNote.Text, "'", "''") & "', " & _
            "UserUltimaModifica = '" & Session("Utente") & "' "

            Dim Command As SqlClient.SqlCommand
            Command = New SqlClient.SqlCommand(strsql, Session("conn"))
            Command.ExecuteNonQuery()
        Else

            lblErrore.Text = "Inserire un Valore Numerico "

        End If
    End Sub

    Private Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    
     
End Class