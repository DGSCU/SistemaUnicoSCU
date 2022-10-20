Imports Logger.Data
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Public Class LogFileConfiguration
End Class
Namespace Output

    Public Class LogFile
        Implements ILogOutput

        Private Const DEFAULT_FILENAME_TEMPLATE As String = "log_@Date@.log"
        Public paths As String()
        Public filenameTemplates As String()
        Public template As String
        Public application As String
        Public daysBeforeDelete As Integer

        Private _AlwaysLog As Boolean = False
        Private _ResetTime As Boolean = False
        Private _LogPreviousExceptions As Boolean = True
        Public Property AlwaysLog As Boolean Implements ILogOutput.AlwaysLog
            Get
                Return _AlwaysLog
            End Get
            Set(value As Boolean)
                _AlwaysLog = value
            End Set
        End Property

        Public Property ResetTime As Boolean Implements ILogOutput.ResetTime
            Get
                Return _ResetTime
            End Get
            Set(value As Boolean)
                _ResetTime = value
            End Set
        End Property

        Public Property LogPreviousExceptions As Boolean Implements ILogOutput.LogPreviousExceptions
            Get
                Return _LogPreviousExceptions
            End Get
            Set(value As Boolean)
                _LogPreviousExceptions = value
            End Set
        End Property


        Public Sub New(ByVal path As String, ByVal fileNameTemplate As String, ByVal templateText As String, ByVal Optional daysBeforeDelete As Integer = 0, ByVal Optional application As String = Nothing)
            Me.daysBeforeDelete = daysBeforeDelete
            Me.application = application
            Dim pathIndexes As List(Of Integer) = New List(Of Integer)()

            For Each level As Integer In [Enum].GetValues(GetType(LogLevel.Levels))
                pathIndexes.Add(level)
            Next

            paths = New String(pathIndexes.Max() + 1 - 1) {}
            filenameTemplates = New String(pathIndexes.Max() + 1 - 1) {}
            pathIndexes.ForEach(Sub(i)
                                    SetPathForLevel(CType([Enum].ToObject(GetType(LogLevel.Levels), i), LogLevel.Levels), path)
                                    SetFileNameTemplateForLevel(CType([Enum].ToObject(GetType(LogLevel.Levels), i), LogLevel.Levels), fileNameTemplate)
                                End Sub)
        End Sub

        Public Sub SetFileNameTemplateForLevel(ByVal level As LogLevel.Levels, ByVal template As String)
            filenameTemplates(CInt(level)) = If(template, DEFAULT_FILENAME_TEMPLATE)
        End Sub

        Public Sub SetPathForLevel(ByVal level As LogLevel.Levels, ByVal path As String)
            If paths.Length < CInt(level) Then
                Throw New Exception("livello di log " & level & " inesistente")
            End If

            If String.IsNullOrEmpty(path) Then
                Throw New Exception("Il path è obbligatorio")
            End If

            Try
                Dim pathExists As Boolean = Directory.Exists(path)
                If Not pathExists Then Directory.CreateDirectory(path)
            Catch ex As Exception
                Throw New Exception("Errore nell'accesso al percorso " & path, ex)
            End Try

            paths(CInt(level)) = path
        End Sub

        Public Sub LogMessage(ByVal log As Log) Implements ILogOutput.LogMessage
            Dim path As String = paths(CInt(log.IdLevel))
            Dim filename As String = If(filenameTemplates(CInt(log.IdLevel)), "")
            Dim durata As String = ""

            If log.StartTime.HasValue Then
                Dim millis As Integer?
                millis = CInt((DateTime.Now - log.StartTime.Value).TotalMilliseconds)
                durata = " (" & millis & " ms)"
            End If

            filename = filename.Replace("@Level@", [Enum].GetName(GetType(LogLevel.Levels), log.IdLevel))
            filename = filename.Replace("@Application@", application)
            Dim filenameTemplate As String = filename

            If Not filename.Contains("@Date@") Then
                filename += DateTime.Today.ToString("yyyyMMdd")
            Else
                filename = filename.Replace("@Date@", DateTime.Today.ToString("yyyyMMdd"))
            End If

            If Not filename.Contains(".") Then
                filename += ".log"
            End If

            Dim fullPath As String = path & "\" & filename

            If daysBeforeDelete > 0 Then
                Dim dir As DirectoryInfo = New DirectoryInfo(path)

                For Each file As FileInfo In dir.GetFiles(filenameTemplate.Replace("@Date@", "????????"))

                    If file.LastWriteTime < DateTime.Now.AddDays(-daysBeforeDelete) Then
                        file.Delete()
                    End If
                Next
            End If

            Dim exception As String = ""

            If log.Exception IsNot Nothing Then
                Dim innerException As String = ""
                If log.Exception.InnerException IsNot Nothing Then
                    innerException = " " & log.Exception.InnerException.Message
                End If

                exception = "- " & log.Exception.Message & innerException & " " & log.Exception.StackTrace
            End If

            Dim method As String = ""

            If Not String.IsNullOrEmpty(log.Action) Then
                method = " (" & log.Action & ")"
            End If

            Dim ipAddress As String = ""

            If Not String.IsNullOrEmpty(log.IpAddress) Then
                ipAddress = " - (" & log.IpAddress & ")"
            End If

            Dim user As String = ""

            If Not String.IsNullOrEmpty(log.Username) Then
                user = " - (" & log.Username & ")"
            End If

            Dim level As String = [Enum].GetName(GetType(LogLevel.Levels), log.IdLevel)

            Using stream As FileStream = New FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, True)

                Using sw As StreamWriter = New StreamWriter(stream)
                    sw.WriteLine(level & method & ":" & user & ipAddress & " - " & Now & durata & " - " & log.Message & exception)
                End Using
            End Using
        End Sub

    End Class
End Namespace
