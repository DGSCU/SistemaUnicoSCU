Public Class Ver_AssegnaVerificatoriSupp
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LblDataInizioPrevistaVer As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataInizioPrevistaVer As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataFinePrevistaVer As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataFinePrevistaVer As System.Web.UI.WebControls.TextBox
    '  Protected WithEvents Image3 As System.Web.UI.WebControls.Image
    Protected WithEvents CmdAssegna As System.Web.UI.WebControls.Button
    Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button
    Protected WithEvents OptPrincipale As System.Web.UI.WebControls.RadioButton
    Protected WithEvents OptSupporto As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    ' Protected WithEvents txtIdVerificatore As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents txtIdVerifica As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblTesto As System.Web.UI.WebControls.Label

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

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            txtIdVerifica.Value = Request.QueryString("IdVerifica")
            txtIdVerificatore.Value = Request.QueryString("IdVerificatore")
            CaricaListaVerificatori()
            AssegnaCheckVerificatore()
            AssegnaDataPrevistaVerifica()
            TxtDataInizioPrevistaVer.ReadOnly = False
            ' Image2.Visible = True
            TxtDataFinePrevistaVer.ReadOnly = False
            '  Image3.Visible = True
        End If
    End Sub

    Private Sub OptPrincipale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptPrincipale.CheckedChanged
        If OptPrincipale.Checked = True Then
            TxtDataInizioPrevistaVer.ReadOnly = False
            '  Image2.Visible = True
            TxtDataFinePrevistaVer.ReadOnly = False
            ' Image3.Visible = True
            lblmessaggio.Text = ""
            ' lblTesto.Text = "principale"
            CaricaListaVerificatori()
            AssegnaCheckVerificatore()
            AssegnaDataPrevistaVerifica()
        End If
    End Sub

    Private Sub OptSupporto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSupporto.CheckedChanged
        If OptSupporto.Checked = True Then
            TxtDataInizioPrevistaVer.Text = ""
            TxtDataFinePrevistaVer.Text = ""
            TxtDataInizioPrevistaVer.ReadOnly = True
            '  Image2.Visible = False
            TxtDataFinePrevistaVer.ReadOnly = True
            ' Image3.Visible = False
            lblmessaggio.Text = ""
            TogliCheck()
            CaricaVerificatoreSupporto()
            'lblTesto.Text = "Supporto"
        End If
    End Sub
    Private Sub CaricaListaVerificatori()
        Dim strsql As String
        Dim dtsVer As DataSet

        strsql = " SELECT IDVerificatore, Cognome + ' ' + Nome AS Verificatore, CASE WHEN Tipologia = 0 THEN 'Interno' ELSE 'IGF' END AS tipologia " & _
                 " FROM  TVerificatori " & _
                 " WHERE GenericoIGF = 0 AND Abilitato = 0  and IdRegCompetenza = " & Session("IdRegCompetenza") & ""
        If OptSupporto.Checked = True Then
            strsql = strsql & "and idVerificatore <> " & txtIdVerificatore.Value & " "
        End If
        dtsVer = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicerca.DataSource = dtsVer
        dgRisultatoRicerca.DataBind()
        TogliCheck()
        If OptPrincipale.Checked = True Then
            AssegnaCheckVerificatore()
        End If
    End Sub
    Private Sub AssegnaCheckVerificatore()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If txtIdVerificatore.Value = item.Cells(0).Text() Then
                check.Checked = True
            End If
        Next
    End Sub
    Private Sub TogliCheck()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            check.Checked = False
        Next
    End Sub
    Private Function VerificaCheck() As Integer
        'controllo è  stata checcato almeno un verificatore peri lsalvataggio
        VerificaCheck = 0
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If check.Checked = True Then
                VerificaCheck = VerificaCheck + 1
            End If
        Next
        Return VerificaCheck
    End Function



    Private Function CaricaVerificatoreSupporto()
        Dim StrSql As String
        Dim dtrVer As SqlClient.SqlDataReader
        Dim IdVerificatore As Integer
        Dim item As DataGridItem
        Dim intX As Integer
        Dim DataSetRicerca As DataSet
        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If
        dgRisultatoRicerca.DataSource = DataSetRicerca
        dgRisultatoRicerca.DataBind()
        CaricaListaVerificatori()
        StrSql = " SELECT TVerificheVerificatori.Idverificatore"
        ',TVerificatori.Cognome + ' ' + TVerificatori.Nome + ' (' + CASE WHEN TVerificatori.Tipologia = 0 THEN 'Interno' ELSE 'IGF' END + ' )' AS Verificatore"
        StrSql = StrSql & " FROM TVerificheVerificatori "
        StrSql = StrSql & " INNER JOIN  TVerificatori ON TVerificheVerificatori.IDVerificatore = TVerificatori.IDVerificatore"
        StrSql = StrSql & " WHERE TVerificheVerificatori.Principale = 0  AND TVerificheVerificatori.IDVerifica = " & txtIdVerifica.Value & " and "
        StrSql = StrSql & " TVerificatori.IDVerificatore <> '" & txtIdVerificatore.Value & "' AND TVerificatori.Abilitato = 0"


        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If
        dtrVer = ClsServer.CreaDatareader(StrSql, Session("conn"))

        Dim vRicVer() As String

        ReDim vRicVer(0)

        vRicVer(0) = ""

        If dtrVer.HasRows = True Then
            Do While dtrVer.Read
                IdVerificatore = dtrVer("Idverificatore")

                If vRicVer(0) = "" Then
                    vRicVer(0) = IdVerificatore
                Else
                    ReDim Preserve vRicVer(UBound(vRicVer) + 1)
                    vRicVer(UBound(vRicVer)) = IdVerificatore
                End If
            Loop
            If Not dtrVer Is Nothing Then
                dtrVer.Close()
                dtrVer = Nothing
            End If
            For Each item In dgRisultatoRicerca.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                For intX = 0 To UBound(vRicVer)
                    If vRicVer(intX) = item.Cells(0).Text() Then
                        check.Checked = True
                    End If
                Next
            Next
        End If
        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If
    End Function
    Private Sub AssegnaDataPrevistaVerifica()
        Dim strsql As String
        Dim dtrGenerico As SqlClient.SqlDataReader

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "Select dbo.FormatoData(DataPrevistaVerifica) AS DataPrevistaVerifica, "
        strsql = strsql & " dbo.FormatoData(DataFinePrevistaVerifica) AS DataFinePrevistaVerifica "
        strsql = strsql & " from TVerifiche where idverifica =" & txtIdVerifica.Value & " "

        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            TxtDataInizioPrevistaVer.Text = dtrGenerico("DataPrevistaVerifica")
            TxtDataFinePrevistaVer.Text = dtrGenerico("DataFinePrevistaVerifica")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Protected Sub CmdAssegna_Click(sender As Object, e As EventArgs) Handles CmdAssegna.Click

        '  Private Sub CmdAssegna_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdAssegna.Click

        Dim CmdGenerico As SqlClient.SqlCommand

        'sqlComOp.Connection = Session("conn")
        'sqlComOp.CommandType = CommandType.Text
        Dim strsql As String

        If VerificaCheck() = 0 Then
            lblmessaggio.Text = "Non è stato selezionato nessun Verificatore."
            Exit Sub
        ElseIf VerificaCheck() > 1 Then
            If OptPrincipale.Checked = True Then
                lblmessaggio.Text = "E' necessario selezionare un solo vericatore."
                Exit Sub
            End If
        End If

        If OptPrincipale.Checked = True Then
            Dim item As DataGridItem
            For Each item In dgRisultatoRicerca.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                If check.Checked = True Then
                    strsql = "UPDATE  tverificheverificatori set idverificatore = " & item.Cells(0).Text & "   where idverifica = " & txtIdVerifica.Value & " and Principale = 1 "
                    CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                    txtIdVerificatore.Value = item.Cells(0).Text
                End If

            Next
            'mod. il 27/02/2008 da simona cordella
            ' modifico la data approvazione
            strsql = "update tverifiche set DataApprovazione =getdate() ,DataAssegnazione =getdate(), DataPrevistaVerifica = '" & TxtDataInizioPrevistaVer.Text & "', DataFinePrevistaVerifica = '" & TxtDataFinePrevistaVer.Text & "' where idverifica = " & txtIdVerifica.Value
            CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        End If

        If OptSupporto.Checked = True Then
            strsql = " delete tverificheverificatori  where idverifica = " & txtIdVerifica.Value & " And Principale = 0"
            CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            Dim item As DataGridItem
            For Each item In dgRisultatoRicerca.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                If check.Checked = True Then
                    strsql = "insert into tverificheverificatori (idverifica,idverificatore,Principale) values( " & txtIdVerifica.Value & "," & item.Cells(0).Text & ",0)"
                    CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                End If
            Next

        End If
        lblmessaggio.Text = "Salvataggio eseguito con successo."
        '  End Sub

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        '  Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.opener.location.reload();" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
        'End Sub
    End Sub
End Class