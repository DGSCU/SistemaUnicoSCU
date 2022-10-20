Imports System.Data
Imports System.Drawing
Public Class Cronologia
    Inherits System.Web.UI.Page

    Dim dtCronologia As DataTable

    Sub ElaborazionePostBind()
        'mette in grassetto tutti i campi diversi dal testo e dalle colonne nascoste se il testo (partendo dall'ultima riga) è diverso dal precedente
        Dim testo As String = ""
        Dim tab As Table = gvCronologia.Controls(0)
        Dim indiceTesto As Integer = tab.Rows(0).Cells.Count - 2

        'Dim tab As Table = gvCronologia.Controls(0)
        'For i = tab.Rows.Count - 1 To 0

        '    If testo <> tab.Rows(i).Cells(indiceTesto).ToString Then
        '        For j = 1 To indiceTesto - 1
        '            tab.Rows(i).Cells(tab.Rows(i).Cells.Count - 2). = HorizontalAlign.Left
        '        Next
        '    End If

        '    tab.Rows(i).Cells(tab.Rows(i).Cells.Count - 1).Visible = False
        '    If i > 0 Then tab.Rows(i).Cells(tab.Rows(i).Cells.Count - 2).HorizontalAlign = HorizontalAlign.Left
        'Next

        For i As Integer = gvCronologia.Rows.Count - 1 To 0 Step -1
            If testo <> gvCronologia.Rows(i).Cells(indiceTesto).Text Then
                testo = gvCronologia.Rows(i).Cells(indiceTesto).Text
                For j = 1 To indiceTesto - 1
                    gvCronologia.Rows(i).Cells(j).Font.Bold = True
                Next
            End If
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            dtCronologia = Session("Cronologia")
            legTitolo.InnerText = Session("TitoloXCronologia")
            gvCronologia.DataSource = dtCronologia
            gvCronologia.DataBind()
            ElaborazionePostBind()
        End If
        'la maschera è pensata per mostrare tutti i dati nel dataset tranne l'ultima colonna
        'nella penultima colonna ci deve essere il testo da mostrare/confrontare
        'nell' ultima colonna ci deve essere l'id della riga (che può essere utile per ulteriori funzionalità)

    End Sub

    Protected Sub gvCronologia_PreRender(sender As Object, e As EventArgs) Handles gvCronologia.PreRender

        'Table tab = GridView1.Controls[0] as Table;

        '  for (int i = 0; i < tab.Rows.Count; i++)
        '  {
        '      tab.Rows[i].Cells[1].Visible = false;
        '  }

        Dim tab As Table = gvCronologia.Controls(0)
        For i = 0 To tab.Rows.Count - 1
            tab.Rows(i).Cells(tab.Rows(i).Cells.Count - 1).Visible = False
            If i > 0 Then tab.Rows(i).Cells(tab.Rows(i).Cells.Count - 2).HorizontalAlign = HorizontalAlign.Left
        Next

    End Sub

    Protected Sub gvCronologia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvCronologia.SelectedIndexChanged
        'For Each row As GridViewRow In gvCronologia.Rows
        '    If row.RowIndex = gvCronologia.SelectedIndex Then
        '        'row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
        '        row.ToolTip = String.Empty
        '    Else
        '        'row.BackColor = ColorTranslator.FromHtml("#FFFFFF")
        '        row.ToolTip = "Clicca per selezionare."
        '    End If
        'Next
    End Sub

    Protected Sub gvCronologia_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCronologia.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvCronologia, "Select$" & e.Row.RowIndex)
        '    e.Row.ToolTip = "Clicca per selezionare."
        'End If
    End Sub

    Protected Sub cmdConfronta_Click(sender As Object, e As EventArgs) Handles cmdConfronta.Click
        lblMessaggio.Visible = False

        Dim _prima As String = ""
        Dim _dopo As String = ""

        If Not GetTestiDaConfrontare(_prima, _dopo) Then
            lblMessaggio.Text = "Selezionare DUE testi da confrontare."
            lblMessaggio.Visible = True
            Exit Sub
        End If

        Session("TestoPrecedenteXCompare") = _prima
        Session("TestoAttualeXCompare") = _dopo
        Session("TitoloXCompare") = Session("TitoloXCronologia")

        'stampo a pagina uno script che mi apre la popup delle variazioni
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""compare.aspx"", """", ""height=500,width=900,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Function GetTestiDaConfrontare(ByRef prima As String, ByRef dopo As String) As Boolean
        Dim _trovati As Integer = 0
        Dim _ret As Boolean = False

        For Each _r As GridViewRow In gvCronologia.Rows
            Dim check As CheckBox = DirectCast(_r.FindControl("chkSel"), CheckBox)
            If Not check Is Nothing AndAlso check.Checked Then
                _trovati += 1

                If _trovati > 2 Then
                    Exit For
                ElseIf _trovati = 1 Then
                    dopo = _r.Cells(_r.Cells.Count - 2).Text    'sono ordinati in modo discendente prendo il testo che è quello successivo
                ElseIf _trovati = 2 Then
                    prima = _r.Cells(_r.Cells.Count - 2).Text    'sono ordinati in modo discendente prendo il testo che è quello precedente
                End If

            End If
        Next

        If _trovati = 2 Then _ret = True

        Return _ret
    End Function
End Class