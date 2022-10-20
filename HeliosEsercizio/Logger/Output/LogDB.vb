Imports Logger.Data
Imports Logger.Output
Imports Newtonsoft.Json
Imports System
Imports System.Data.SqlClient

Public Class LogDB
	Implements ILogOutput

	Private ReadOnly connectionString As String
	Public Sub New(ByVal connectionString As String)
		Me.connectionString = connectionString
	End Sub

	Private _AlwaysLog As Boolean = True
	Private _ResetTime As Boolean = False
	Private _LogPreviousExceptions As Boolean = False
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

	Public Sub LogMessage(ByVal log As Log) Implements ILogOutput.LogMessage
		Dim guid As Guid = Nothing

		Try

			Dim exception As String = Nothing

            If log.Exception IsNot Nothing Then
                Dim innerException As String = ""
                If log.Exception.InnerException IsNot Nothing Then
                    innerException = " " & log.Exception.InnerException.Message
                End If

                exception = log.Exception.Message & innerException & " " & log.Exception.StackTrace
            End If

            Dim sessionId As Guid? = Nothing

            If guid.TryParse(log.SessionId, guid) Then
                sessionId = guid
            End If

            Dim query As String = String.Empty
            query &= "INSERT INTO Log (TimeStamp,StartTime,Duration,IdLevel,IpAddress,Username,Name,CodiceFiscaleEnte,IdEventType,EntityId,EntityName,ApplicationName,Action,Controller,Method,Message,Exception,Parameter,SessionId)  "
            query &= "VALUES (@TimeStamp,@StartTime,@Duration,@IdLevel,@IpAddress,@Username,@Name,@CodiceFiscaleEnte,@IdEventType,@EntityId,@EntityName,@ApplicationName,@Action,@Controller,@Method,@Message,@Exception,@Parameter,@SessionId)"

            Using conn As New SqlConnection(connectionString)
                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@TimeStamp", Now)
                        .Parameters.AddWithValue("@StartTime", If(log.StartTime, DBNull.Value))
                        .Parameters.AddWithValue("@Duration", If(log.Duration, DBNull.Value))
                        .Parameters.AddWithValue("@IdLevel", log.IdLevel)
                        .Parameters.AddWithValue("@IpAddress", If(log.IpAddress, DBNull.Value))
                        .Parameters.AddWithValue("@Username", If(log.Username, DBNull.Value))
                        .Parameters.AddWithValue("@Name", If(log.Name, DBNull.Value))
                        .Parameters.AddWithValue("@CodiceFiscaleEnte", If(log.CodiceFiscaleEnte, DBNull.Value))
                        .Parameters.AddWithValue("@IdEventType", If(log.IdEventType, DBNull.Value))
                        .Parameters.AddWithValue("@EntityId", If(log.EntityId, DBNull.Value))
                        .Parameters.AddWithValue("@EntityName", If(log.EntityName, DBNull.Value))
                        .Parameters.AddWithValue("@ApplicationName", If(log.ApplicationName, DBNull.Value))
                        .Parameters.AddWithValue("@Action", If(log.Action, DBNull.Value))
                        .Parameters.AddWithValue("@Controller", If(log.Controller, DBNull.Value))
                        .Parameters.AddWithValue("@Method", If(log.Method, DBNull.Value))
                        .Parameters.AddWithValue("@Message", If(log.Message, DBNull.Value))
                        .Parameters.AddWithValue("@Exception", If(exception, DBNull.Value))
                        .Parameters.AddWithValue("@Parameter", If(log.Parameter, DBNull.Value))
                        .Parameters.AddWithValue("@SessionId", If(log.SessionId, DBNull.Value))
                    End With


                    conn.Open()
                    comm.ExecuteNonQuery()
                End Using
            End Using



        Catch e As Exception
            Throw e
        End Try
	End Sub
End Class
