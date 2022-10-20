Imports System.Collections.Generic

Public Class Helper
    Public Function GetCheckBoxByValidationGroup(ByVal parentControl As Control, ByVal groupName As String) As List(Of CheckBox)
        Dim result As New List(Of CheckBox)()
        GetCheckBoxByValidationGroup(parentControl, groupName, result)
        Return result
    End Function
    Public Sub GetCheckBoxByValidationGroup(ByVal parentControl As Control, ByVal groupName As String, ByVal list As List(Of CheckBox))
        For Each c As Control In parentControl.Controls
            If TypeOf c Is CheckBox AndAlso DirectCast(c, CheckBox).ValidationGroup.Equals(groupName) Then
                list.Add(TryCast(c, CheckBox))
            ElseIf c.HasControls() Then
                GetCheckBoxByValidationGroup(c, groupName, list)
            End If
        Next
    End Sub

#Region "Design"
    Public Function CreaDivRigaVuota() As Panel
        Dim panelRigaVuota As Panel = New Panel
        panelRigaVuota.CssClass = "RigaVuota"
        Return panelRigaVuota
    End Function

    Public Sub AggiungiRigaVuota(ByRef contenitore As Panel)
        Dim panelRow As Panel = New Panel()
        panelRow.CssClass = "rigaVuota"
        Dim label As Label = New Label()
        label.Text = "&nbsp;"
        panelRow.Controls.Add(label)
        contenitore.Controls.Add(panelRow)
    End Sub

    Sub ModificaStyleDatiModificati(ByRef variabileModificata As WebControl)
        If variabileModificata.GetType Is GetType(ImageButton) Or variabileModificata.GetType Is GetType(CheckBox) Then
            variabileModificata.Style.Add("border", "solid")
        End If

        variabileModificata.Style.Add("border-color", "Red")
        variabileModificata.ToolTip = variabileModificata.ToolTip & "(Dato Modificato)"
    End Sub

    Sub RipristinaStyleDatiModificati(ByRef variabileModificata As WebControl)
        If variabileModificata.GetType Is GetType(ImageButton) Then
            variabileModificata.Style.Add("border", "none")
        End If
        variabileModificata.Style.Add("border-color", "none")
        variabileModificata.ToolTip = variabileModificata.ToolTip & "(Dato Modificato)"
    End Sub
#End Region
End Class
