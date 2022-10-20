Public Class clsConversione

    Public Shared Function DaNullInNothing(ByVal ObjX As Object) As Object
        If ObjX Is DBNull.Value Then
            Return Nothing
        Else
            Return ObjX
        End If
    End Function



   

    Public Shared Function DaNullInStringaVuota(ByVal ObjX As Object) As String
        If ObjX Is Nothing Then
            Return ""
        Else
            Return ObjX.ToString
        End If
    End Function

    Public Shared Function DaVuotoInNull(ByVal ObjX As Object) As Object
        If ObjX Is Nothing OrElse ObjX.ToString = "" Then
            Return DBNull.Value
        Else
            Return ObjX
        End If
    End Function

    Public Shared Function DaZeroInNull(ByVal ObjX As Object) As Object
        If ObjX Is Nothing OrElse ObjX = 0 Then
            Return DBNull.Value
        Else
            Return ObjX
        End If
    End Function
    Public Shared Function DaZeroInStringa(ByVal Objx As Object) As String
        If Objx Is Nothing OrElse Objx = 0 Then
            Return ""
        Else
            Return Objx.ToString
        End If
    End Function

    Public Shared Function DaBitAInteger(ByVal ObjX) As Object
        If ObjX = -1 OrElse ObjX = 1 Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Public Shared Function DaStringaInInteger(ByVal ObjX As Object) As Integer
        Return CType(ObjX, Integer)
    End Function

    Public Shared Function DaStringaInData(ByVal ObjX As String) As Object
        If ObjX Is Nothing OrElse ObjX = "" Then
            Return DBNull.Value
        Else
            Return ObjX
        End If
    End Function

    Public Shared Function DaStringaInNothing(ByVal Objx As String) As Object
        If Objx = "" Then
            Return Nothing
        Else
            Return (Objx)
        End If
    End Function

End Class
