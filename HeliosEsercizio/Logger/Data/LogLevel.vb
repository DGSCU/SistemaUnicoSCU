Namespace Data
    Public Class LogLevel
        Public Const TRACE As Integer = 1
        Public Const INFORMATION As Integer = 2
        Public Const WARNING As Integer = 3
        Public Const [ERROR] As Integer = 4
        Public Const CRITICAL As Integer = 5

        Public Enum Levels
            DebugLevel = TRACE
            InformationLevel = INFORMATION
            WarningLevel = WARNING
            ErrorLevel = [ERROR]
            CriticalLevel = CRITICAL
        End Enum

        Public Property Id As Integer
        Public Property Name As String
    End Class
End Namespace
