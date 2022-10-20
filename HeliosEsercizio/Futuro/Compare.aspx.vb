'Imports Microsoft.Security.Application
Imports DiffMatchPatch
Imports System.Drawing

Public Class Compare
    Inherits System.Web.UI.Page

    'this is the diff object;
    Private ReadOnly _diff As diff_match_patch = New diff_match_patch()

    'these are the diffs
    Private _diffs As List(Of Diff)

    'chunks for formatting the two strings:
    Private _chunklist1 As List(Of Chunk)
    Private _chunklist2 As List(Of Chunk)

    'two color lists:
    ReadOnly _colors1 As Color() = {Color.LightGreen, Color.LightSalmon, Color.White}
    ReadOnly _colors2 As Color() = {Color.LightSalmon, Color.LightGreen, Color.White}

    Shared Function GetRGBA(c As Color) As String

        'Dim _s As String = c.ToKnownColor.ToString.IndexOf("{") - 1)

    End Function

    Public Structure Chunk
        Public Startpos As Integer
        Public Length As Integer
        Public BackColor As Color
        Public testo As String
    End Structure

    Dim stringa1 As String
    Dim stringa2 As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        stringa1 = Session("TestoPrecedenteXCompare")
        stringa2 = Session("TestoAttualeXCompare")

        legTitolo.InnerText = Session("TitoloXCompare")

        'stringa1 = Encoder.HtmlEncode(stringa1)
        'stringa2 = Encoder.HtmlEncode(stringa2)

        If Not Page.IsPostBack Then
            Radio2.Checked = True
            chkVariazioni.Checked = True
        End If
        CompareText()

    End Sub


    Sub CompareText()
        _diffs = _diff.diff_main(stringa1, stringa2)
        _diff.diff_cleanupSemanticLossless(_diffs)

        If Radio1.Checked Then
            '_chunklist1 = CollectChunks(stringa1, True)
            'divTesto2.InnerHtml = PaintChunks(_chunklist1).Replace(vbCrLf, "<br>")
            divTesto2.InnerHtml = stringa1.Replace(vbCrLf, "<br>").Replace(vbTab, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            chkVariazioni.Visible = False
            label1.Visible = False
            label2.Visible = False
        Else
            _chunklist2 = CollectChunks(stringa2, False)
            divTesto2.InnerHtml = PaintChunks(_chunklist2).Replace(vbCrLf, "<br>").Replace(vbTab, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            chkVariazioni.Visible = True
            If chkVariazioni.Checked Then
                label1.Visible = True
                label2.Visible = True
            Else
                label1.Visible = False
                label2.Visible = False
            End If
        End If

    End Sub

    Private Function CollectChunks(testo As String, isTestoDiPartenza As Boolean) As List(Of Chunk)
        Dim testoInElaborazione As StringBuilder = New StringBuilder()

        Dim chunkList As List(Of Chunk) = New List(Of Chunk)()

        For Each d As Diff In _diffs
            If Not isTestoDiPartenza AndAlso d.operation = Operation.DELETE AndAlso Not chkVariazioni.Checked Then Continue For
            If isTestoDiPartenza AndAlso d.operation = Operation.INSERT Then Continue For

            Dim ch As Chunk = New Chunk()
            Dim length As Integer = testoInElaborazione.Length
            testoInElaborazione.Append(d.text)
            ch.Startpos = length
            ch.Length = d.text.Length
            ch.BackColor = If(isTestoDiPartenza, _colors1(CInt(Operation.EQUAL)), If(chkVariazioni.Checked, _colors2(CInt(d.operation)), _colors2(CInt(Operation.EQUAL))))
            ch.testo = d.text
            chunkList.Add(ch)
        Next

        Return chunkList
    End Function


    Private Function PaintChunks(ByVal theChunks As IEnumerable(Of Chunk)) As String
        Dim _elab As StringBuilder = New StringBuilder()
        For Each ch As Chunk In theChunks
            _elab.Append("<span style=background-color:" + ch.BackColor.ToKnownColor.ToString + ";>")
            _elab.Append(ch.testo)
            _elab.Append("</span>")
        Next
        Return _elab.ToString()
    End Function

End Class