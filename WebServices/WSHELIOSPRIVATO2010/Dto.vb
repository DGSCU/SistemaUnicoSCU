Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Public Class ResponseData
    Public Property Success As Boolean
    Public Property Message As String
End Class

Public Class inputLoginVolontario
    Public Property userName As String
    Public Property password As String
End Class

Public Class inputCambioPassword
    Public Property userName As String
    Public Property passwordAttuale As String
    Public Property password As String
End Class

Public Class inputRecuperoPassword
    Public Property codiceFiscale As String
    Public Property returnUrl As String
End Class

