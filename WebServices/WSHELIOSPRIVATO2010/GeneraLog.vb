Public Class GeneraLog
    '*******************************************************
    Private ReadOnly FileName As String
    Private ReadOnly FileSystemPath As String
    Private ReadOnly TraceError As String
    Private ReadOnly TipoProcesso As String
    Private myPath As New System.Web.UI.Page
    '*******************************************************
    Public Sub New(ByRef strError As String, ByVal strProcesso As String)

        Me.FileName = "WS_Log.txt"

        Me.FileSystemPath = myPath.Server.MapPath("LOG")

        Me.TraceError = strError

        Me.TipoProcesso = strProcesso

    End Sub

    '*******************************************************
    Public Function GeneraFileLog() As String

        Dim writer As IO.StreamWriter
        Dim i, j, k As Integer

        Try
            'se il file non esiste lo creo
            If Not IO.File.Exists(Me.FileSystemPath & "\" & Me.FileName) Then
                writer = IO.File.CreateText(Me.FileSystemPath & "\" & Me.FileName)
            Else
                writer = New IO.StreamWriter(Me.FileSystemPath & "\" & Me.FileName, True)
            End If

            writer.WriteLine("Data: " & Format(Now, "dd-MM-yyyy hh-mm-ss"))
            writer.WriteLine("Processo: " & TipoProcesso)
            writer.WriteLine("Errore: " & TraceError)
            writer.WriteLine("----------------------------------------------------------------------")
            writer.Close()

            Return ""
        Catch ex As Exception
            Return "Errore durante la generazione del file LOG. Messaggio dell'eccezione: " & ex.Message
        End Try

    End Function

End Class
