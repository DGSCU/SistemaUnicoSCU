Public Class GuidSessionIDManager
    Inherits SessionIDManager

    Public Overrides Function CreateSessionID(ByVal context As HttpContext) As String
        Return Guid.NewGuid().ToString()
    End Function

    Public Overrides Function Validate(ByVal id As String) As Boolean
        Try
            Dim testGuid As Guid = New Guid(id)
            If id = testGuid.ToString() Then Return True
        Catch
        End Try

        Return False
    End Function
End Class
