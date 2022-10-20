' Classe con funzioni shared utili a gestire i componenti delle webform e winform
Public Class clsGui

    Public Shared Sub CaricaDropDown(ByRef ddX As DropDownList, ByVal Dtx As DataTable, ByVal CampoVisuale As String, ByVal CampoInterno As String, Optional ByVal RigaBianca As Boolean = True)
        ddX.Items.Clear()
        ddX.Items.Add("")
        For I As Integer = 0 To Dtx.Rows.Count - 1
            ddX.Items.Add(Dtx.Rows(I)(CampoVisuale))
            ddX.Items(I + 1).Value = Dtx.Rows(I)(CampoInterno)
        Next
    End Sub

    Public Shared Sub EliminaItemDropDrown(ByRef ddX As DropDownList, ByVal ValoreInternoX As String)
        If Not ddX.Items.FindByValue(ValoreInternoX) Is Nothing Then
            ddX.Items.Remove(ddX.Items.FindByValue(ValoreInternoX))
        End If
    End Sub

    Public Shared Sub SvuotaCampi(ByRef WebFormX As Object)
        For Each Ctrl As Control In WebFormX.Controls
            If Ctrl.Controls.Count > 0 Then
                SvuotaCampi(Ctrl)
            Else
                Select Case Ctrl.GetType.Name
                    Case GetType(TextBox).Name
                        CType(Ctrl, TextBox).Text = ""
                    Case GetType(DropDownList).Name
                        CType(Ctrl, DropDownList).SelectedIndex = 0
                End Select
            End If
        Next
    End Sub



End Class
