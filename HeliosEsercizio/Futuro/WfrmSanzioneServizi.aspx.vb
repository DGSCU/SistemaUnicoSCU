Public Class WfrmSanzioneServizi
    Inherits System.Web.UI.Page
    '********************************
    ' MASCHERA CREATA DA SIMONA CORDELLA IL 18/02/2011
    ' GESTIONE SANZIONE SERVIZI
    '********************************
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents dgElencoServizi As System.Web.UI.WebControls.DataGrid

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaServizi()
        End If
    End Sub

    Private Sub CaricaServizi()
        Dim strSql As String
        Dim dtsServizi As DataSet
        Dim dtrServizi As SqlClient.SqlDataReader
        Dim item As DataGridItem


        Dim idModello As Integer
        If Session("Sistema") = "Helios" Then
            idModello = 94
        Else
            idModello = 237
        End If


        strSql = "Select IdServizio,Descrizione from TServiziSiged "
        dtsServizi = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgElencoServizi.DataSource = dtsServizi
        dgElencoServizi.DataBind()

        If Not dtrServizi Is Nothing Then
            dtrServizi.Close()
            dtrServizi = Nothing
        End If

        strSql = " Select IdServizio  from TVerificheServizi where IDModello = " & idModello & " AND IdVerifica =" & Request.QueryString("IDVerifica") & " "
        dtrServizi = ClsServer.CreaDatareader(strSql, Session("conn"))
        While dtrServizi.Read()
            For Each item In dgElencoServizi.Items
                Dim check As CheckBox = DirectCast(item.FindControl("ChkServizi"), CheckBox)
                If dtrServizi("IdServizio") = item.Cells(2).Text Then
                    check.Checked = True
                End If
            Next
        End While
        If Not dtrServizi Is Nothing Then
            dtrServizi.Close()
            dtrServizi = Nothing
        End If

    End Sub

    Private Sub DeleteSanzioneServizi(ByVal idModello As Integer)
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strSql As String
        strSql = "DELETE FROM TVerificheServizi WHERE IDMODELLO = " & idModello & " AND IDVERIFICA =" & Request.QueryString("IDVerifica") & " "
        CmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub

    Private Sub InserimentoSanzioneServizi(ByVal idModello As Integer)
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strSql As String
        Dim item As DataGridItem
        For Each item In dgElencoServizi.Items
            Dim check As CheckBox = DirectCast(item.FindControl("ChkServizi"), CheckBox)
            If check.Checked = True Then
                strSql = "INSERT INTO  TVerificheServizi (IDModello, IDVerifica,IDServizio,Username,Data)  " & _
                        " VALUES(" & idModello & " , " & Request.QueryString("IDVerifica") & ", " & _
                        "         " & item.Cells(2).Text & ",   '" & Session("UTENTE") & " ', getdate())"

                CmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))
            End If
        Next
        LblInfo.Visible = True
        LblInfo.Text = "Associazione al servizio eseguito con successo."
    End Sub


    Private Sub btSalva_Click(sender As Object, e As System.EventArgs) Handles btSalva.Click
        Dim idModello As Integer
        If Session("Sistema") = "Helios" Then
            idModello = 94
        Else
            idModello = 237
        End If
        DeleteSanzioneServizi(idModello)
        InserimentoSanzioneServizi(idModello)
    End Sub

    Protected Sub dgElencoServizi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgElencoServizi.SelectedIndexChanged

    End Sub
End Class