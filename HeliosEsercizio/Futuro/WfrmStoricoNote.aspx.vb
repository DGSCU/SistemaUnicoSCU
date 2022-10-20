Public Class WfrmStoricoNote
    Inherits System.Web.UI.Page
    Dim strSql As String
    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim cmdGenerico As SqlClient.SqlCommand


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents txtNuovaNota As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgStoricoNote As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton

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
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            'IdAttivitàSedeAssegnazione Session("IdASA")
            CaricaDataGrid(dtgStoricoNote)
        End If
    End Sub

    Private Sub dtgStoricoNote_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgStoricoNote.PageIndexChanged
        dtgStoricoNote.SelectedIndex = -1
        dtgStoricoNote.EditItemIndex = -1
        dtgStoricoNote.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dtgStoricoNote)
    End Sub

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid)
        strSql = "Select * from CronologiaNoteGraduatoria where IDAttivitàSedeAssegnazione ='" & Session("IdASA") & "' Order by IDCronologiaNoteGraduatoria desc"
        GridDaCaricare.DataSource = ClsServer.DataSetGenerico(strSql, Session("conn"))
        GridDaCaricare.DataBind()
        GridDaCaricare.Visible = True
    End Sub


    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSalva.Click
        If txtNuovaNota.Text.Trim <> "" Then
            strSql = "Insert Into CronologiaNoteGraduatoria (IDAttivitàSedeAssegnazione,NoteGraduatoria,DataNota,UserNameNota) Values ('" & Session("IdASA") & "','" & ClsServer.NoApice(txtNuovaNota.Text) & "', getdate(),'" & Session("Utente") & "')"
            Dim cmdNote As New SqlClient.SqlCommand
            cmdNote.CommandText = strSql
            cmdNote.Connection = Session("Conn")
            cmdNote.ExecuteNonQuery()

            strSql = "Update AttivitàSediAssegnazione Set NoteGraduatoria ='" & ClsServer.NoApice(txtNuovaNota.Text) & "', DataNota=getdate(), UserNameNota='" & Session("Utente") & "' Where IDAttivitàSedeAssegnazione='" & Session("IdASA") & "'"
            cmdNote.CommandText = strSql
            cmdNote.Connection = Session("Conn")
            cmdNote.ExecuteNonQuery()

            CaricaDataGrid(dtgStoricoNote)

            txtNuovaNota.Text = ""
            If Not cmdNote Is Nothing Then
                cmdNote.Dispose()
                cmdNote = Nothing
            End If

            Response.Redirect("WfrmAssociaVolontari.aspx")
        Else
            Response.Write("<script>")
            Response.Write("alert('Inserire il testo della nota prima di salvare.');")
            Response.Write("<script>")
        End If
    End Sub
    Private Sub dtgStoricoNote_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStoricoNote.ItemCommand
        If e.CommandName = "Rimuovi" Then
            strSql = "delete CronologiaNoteGraduatoria where idCronologiaNoteGraduatoria='" & e.Item.Cells(0).Text & "'"
            cmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))
            'FZ 08/01/07
            'In fase di cancellazione delle note, aggiorno il campo noteGRADUATORIA in ATTIVITàSEDIASSEGNAZIONE
            'Con l'ultima nota rimasta salvata. Se non ci sono piu' note, allora imposto il campo a NULL
            'Leggo in CRONOLOGIANOTEGRADUATORIA tutti i record legati alla graduatoria presa in esame
            Dim dtrLeggiDati As SqlClient.SqlDataReader
            strSql = "SELECT TOP 1 IDCronologiaNoteGraduatoria, IDAttivitàSedeAssegnazione, NoteGraduatoria " & _
                     "FROM CronologiaNoteGraduatoria WHERE (IDAttivitàSedeAssegnazione = " & Session("IdASA") & ") " & _
                     "ORDER BY IDCronologiaNoteGraduatoria DESC"

            dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrLeggiDati.Read = True Then
                'Se c'è una nota rimasta allora faccio l'update su ATTIVITàSEDIASSEGNAZIONE
                Dim AppoNOte As String = dtrLeggiDati("NoteGraduatoria")

                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
                strSql = "UPDATE AttivitàSediAssegnazione SET NOTEGRADUATORIA = '" & AppoNOte & "' " & _
                "WHERE IDAttivitàSedeAssegnazione = " & Session("IdASA")


                'Eseguo lo script di aggiornamento della pagina sottostante
                Response.Write("<SCRIPT>" & vbCrLf)
                Response.Write("window.opener.document.all.txtNote.value ='" & AppoNOte & "';" & vbCrLf)
                Response.Write("</SCRIPT>")

                Dim Command As SqlClient.SqlCommand
                Command = New SqlClient.SqlCommand(strSql, Session("conn"))
                Command.ExecuteNonQuery()

            Else
                'non ci sono più note salvate, quindi imposto il campo su ATTIVITàSEDIASSEGNAZIONE a NULL

                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
                strSql = "UPDATE AttivitàSediAssegnazione SET NOTEGRADUATORIA = NULL " & _
                "WHERE IDAttivitàSedeAssegnazione = " & Session("IdASA")
                'Eseguo lo script di aggiornamento della pagina sottostante
                Response.Write("<SCRIPT>" & vbCrLf)
                Response.Write("window.opener.document.all.txtNote.value ='';" & vbCrLf)
                Response.Write("</SCRIPT>")
                Dim Command As SqlClient.SqlCommand
                Command = New SqlClient.SqlCommand(strSql, Session("conn"))
                Command.ExecuteNonQuery()


            End If
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If
            'FZ fine


        End If
        CaricaDataGrid(dtgStoricoNote)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

End Class