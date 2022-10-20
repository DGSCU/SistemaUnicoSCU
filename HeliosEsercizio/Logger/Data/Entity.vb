Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Data
    Public Class Entity
        Public Sub New()
        End Sub

        Public Sub New(ByVal id As Integer, ByVal name As String)
            id = id
            name = name
        End Sub

        Public Property Id As Integer
        Public Property Name As String
    End Class
End Namespace
