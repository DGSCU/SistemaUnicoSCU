Imports Logger.Data
Imports Logger.Output
Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Threading
Imports System.Threading.Tasks

Public Class Logger
	Private Shared ReadOnly outputs As List(Of ILogOutput) = New List(Of ILogOutput)()

	Public Shared Sub AddOutput(ByVal output As ILogOutput)
		outputs.Add(output)
	End Sub

	Private Shared applicationName As String

	Public Shared Sub SetApplicationName(ByVal applicationName As String)
		Logger.applicationName = applicationName
	End Sub

	Private startTime As DateTime?
	Private ReadOnly controller As String
	Private ReadOnly action As String
	Private ReadOnly method As String
	Private username As String
	Private name As String
	Private ente As String

	Public Sub SetUsername(ByVal username As String, Optional ByVal name As String = Nothing)
		Me.username = username
		Me.name = name
	End Sub

	Public Sub SetEnte(ByVal ente As String)
		Me.ente = ente
	End Sub

	Private ReadOnly idUser As Integer?
	Private ReadOnly ipAddress As String
	Private ReadOnly applicationIpAddress As String
	Private ReadOnly applicationHost As String
	Private ReadOnly sessionId As String

	Public Sub New(ByVal controller As String, ByVal action As String, ByVal method As String, Optional ByVal username As String = Nothing, Optional ByVal name As String = Nothing, Optional ByVal ente As String = Nothing, Optional ByVal idUser As Integer? = Nothing, Optional ByVal startTime As DateTime? = Nothing, Optional ByVal ipAddress As String = Nothing, Optional ByVal applicationIpAddress As String = Nothing, Optional ByVal applicationHost As String = Nothing, Optional ByVal sessionId As String = Nothing)
		Me.startTime = If(startTime, DateTime.Now)
		Me.controller = controller
		Me.action = action
		Me.method = method
		Me.idUser = idUser
		Me.username = username
		Me.name = name
		Me.ente = ente
		Me.ipAddress = ipAddress
		Me.applicationIpAddress = applicationIpAddress
		Me.applicationHost = applicationHost
		Me.sessionId = sessionId
	End Sub


	Private Sub LogMessage(ByVal log As Log)
		log.Controller = controller
		log.Action = action
		log.Method = method
		log.IdUser = idUser
		log.Username = username
		log.Name = name
		log.CodiceFiscaleEnte = ente
		log.StartTime = startTime
		log.ApplicationName = applicationName
		log.IpAddress = ipAddress
		log.ApplicationIpAddress = applicationIpAddress
		log.ApplicationHost = applicationHost
		log.SessionId = sessionId
		Dim logged As Boolean = False
		Dim previousException As Exception = Nothing

		For Each logOutput As ILogOutput In outputs

			If logOutput.LogPreviousExceptions AndAlso previousException IsNot Nothing Then
				logOutput.LogMessage(New Log() With {
				 .IdLevel = LogLevel.[ERROR],
				 .Message = "Errore nella scrittura del log",
				 .Exception = previousException
				})
			End If

			If logged AndAlso Not logOutput.AlwaysLog Then
				Continue For
			End If

			Try
				logOutput.LogMessage(log)
				logged = True
			Catch e As Exception
				previousException = e
				logged = False
			End Try

			If logOutput.ResetTime Then startTime = DateTime.Now
		Next
	End Sub

	Public Sub Trace(ByVal message As String, Optional ByVal idEventType As Integer? = Nothing, Optional ByVal parameters As Object = Nothing, Optional ByVal entity As Entity = Nothing)
		Dim entityId As Integer? = Nothing
		Dim entityName As String = Nothing
		If entity IsNot Nothing Then
			entityId = entity.Id
			entityName = entity.Name
		End If
		Dim newThread = New Thread(AddressOf LogMessage)
		newThread.Start(New Log() With {
			.IdLevel = LogLevel.TRACE,
			.Message = message,
			.EntityId = entityId,
			.EntityName = entityName,
			.IdEventType = idEventType,
			.Parameter = ConvertToJson(parameters)
		})
	End Sub

	Public Sub Information(ByVal idEventType As Integer, Optional ByVal message As String = Nothing, Optional ByVal parameters As Object = Nothing, Optional ByVal entity As Entity = Nothing)
		Dim entityId As Integer? = Nothing
		Dim entityName As String = Nothing
		If entity IsNot Nothing Then
			entityId = entity.Id
			entityName = entity.Name
		End If
		Dim newThread = New Thread(AddressOf LogMessage)
		newThread.Start(New Log() With {
			.IdLevel = LogLevel.INFORMATION,
			.Message = message,
			.EntityId = entityId,
			.EntityName = entityName,
			.IdEventType = idEventType,
			.Parameter = ConvertToJson(parameters)
		})

	End Sub

	Public Sub Warning(ByVal idEventType As Integer, Optional ByVal message As String = Nothing, Optional ByVal parameters As Object = Nothing, Optional ByVal entity As Entity = Nothing)
		Dim entityId As Integer? = Nothing
		Dim entityName As String = Nothing
		If entity IsNot Nothing Then
			entityId = entity.Id
			entityName = entity.Name
		End If
		Dim newThread = New Thread(AddressOf LogMessage)
		newThread.Start(New Log() With {
			.IdLevel = LogLevel.WARNING,
			.Message = message,
			.EntityId = entityId,
			.EntityName = entityName,
			.IdEventType = idEventType,
			.Parameter = ConvertToJson(parameters)
		})

	End Sub

	Public Sub [Error](ByVal idEventType As Integer, Optional ByVal message As String = Nothing, Optional ByVal exception As Exception = Nothing, Optional ByVal parameters As Object = Nothing, Optional ByVal entity As Entity = Nothing)
		Dim entityId As Integer? = Nothing
		Dim entityName As String = Nothing
		If entity IsNot Nothing Then
			entityId = entity.Id
			entityName = entity.Name
		End If
		Dim newThread = New Thread(AddressOf LogMessage)
		newThread.Start(New Log() With {
			.IdLevel = LogLevel.[ERROR],
			.Message = message,
			.EntityId = entityId,
			.EntityName = entityName,
			.IdEventType = idEventType,
			.Exception = exception,
			.Parameter = ConvertToJson(parameters)
		})

	End Sub

	Public Sub Critical(ByVal idEventType As Integer, Optional ByVal message As String = Nothing, Optional ByVal exception As Exception = Nothing, Optional ByVal parameters As Object = Nothing, Optional ByVal entity As Entity = Nothing)
		Dim entityId As Integer? = Nothing
		Dim entityName As String = Nothing
		If entity IsNot Nothing Then
			entityId = entity.Id
			entityName = entity.Name
		End If
		Dim newThread = New Thread(AddressOf LogMessage)
		newThread.Start(New Log() With {
			.IdLevel = LogLevel.CRITICAL,
			.Message = message,
			.EntityId = entityId,
			.EntityName = entityName,
			.IdEventType = idEventType,
			.Exception = exception,
			.Parameter = ConvertToJson(parameters)
		})

	End Sub

	Private Function ConvertToJson(item As Object) As String
		Dim jsonParameters As String = Nothing
		If item IsNot Nothing Then
			If GetType(NameValueCollection).IsAssignableFrom(item.GetType) Then
				Dim keys As NameValueCollection = item
				jsonParameters = "{"
				For Each key As String In keys
					jsonParameters &= """" & key & """ : """ & keys(key) & ""","
				Next
				If keys.Count > 0 Then
					jsonParameters = jsonParameters.Substring(0, jsonParameters.Length - 1)
				End If
				jsonParameters &= "}"

			Else
				jsonParameters = JsonConvert.SerializeObject(item)
			End If
		End If
		Return jsonParameters
	End Function


End Class
