Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Text

Namespace Data
    Public Class Log
        Public Property Id As Integer

        Public ReadOnly Property TimeStamp As DateTime
            Get
                Return DateTime.Now
            End Get
        End Property

        Public Property StartTime As DateTime?

        Public ReadOnly Property Duration As Integer?
            Get

                If Not StartTime.HasValue Then
                    Return Nothing
                End If

                Return Convert.ToInt32((DateTime.Now - StartTime.Value).TotalMilliseconds)
            End Get
        End Property

        Public Property IdLevel As Integer
        Public Property IpAddress As String
        Public Property Username As String
        Public Property Name As String
        Public Property CodiceFiscaleEnte As String
        Public Property IdUser As Integer?
        Public Property IdEventType As Integer?
        Public Property EntityId As Integer?
        Public Property EntityName As String
        Public Property ApplicationIpAddress As String
        Public Property ApplicationHost As String
        Public Property ApplicationName As String
        Public Property SessionId As String
        Public Property Controller As String
        Public Property Action As String
        Public Property Method As String
        Public Property Message As String
        Public Property Exception As Exception
        Public Property Parameter As String
    End Class
End Namespace
