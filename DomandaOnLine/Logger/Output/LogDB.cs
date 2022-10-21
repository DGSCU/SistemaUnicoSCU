using Logger.Data;
using Newtonsoft.Json;
using System;

namespace Logger.Output
{
	public class LogDB : ILogOutput
	{
		public LogDB(string connectionString)
		{
		}

		public bool AlwaysLog => true;

		public bool LogPreviousExceptions => false;
		public bool ResetTime => true;

		public void LogMessage(Log log)
		{
			try
			{
				LogContext context = new LogContext();
				string exception = null;
				if (log.Exception != null)
				{
					exception = $"{log.Exception.Message} {log.Exception.InnerException?.Message} {log.Exception.StackTrace}";
				}
				Guid? sessionId = null;
				if (Guid.TryParse(log.SessionId, out Guid guid)){
					sessionId = guid;
				}
				var logTable = context.Log.Add(new LogTable()
				{
					IdLevel = (int)log.IdLevel,
					IpAddress = log.IpAddress,
					Username = log.Username,
					Name = log.Name,
					CodiceFiscaleEnte = log.CodiceFiscaleEnte,
					TimeStamp = log.TimeStamp,
					StartTime = log.StartTime,
					Duration = log.Duration,
					IdEventType = log.IdEventType,
					EntityId = log.EntityId,
					EntityName = log.EntityName,
					Message = log.Message,
					ApplicationName = log.ApplicationName,
					Method = log.Method,
					Controller = log.Controller,
					Action = log.Action,
					Exception = exception,
					Parameter = log.Parameter,
					SessionId = sessionId
				});
				context.SaveChanges();
			}
			catch (Exception e)
			{

				throw e;
			}
		}

	}
}
