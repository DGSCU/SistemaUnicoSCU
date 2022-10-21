using Logger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Logger.Output
{
	public class LogDB : ILogOutput
	{
		private DbContextOptions<LogContext> options;
		public LogDB(string connectionString)
		{
			DbContextOptionsBuilder<LogContext> optionsBuilder = new DbContextOptionsBuilder<LogContext>();
			options = optionsBuilder.UseSqlServer(connectionString).Options;
		}

		public bool AlwaysLog => true;

		public bool LogPreviousExceptions => false;
		public bool ResetTime => true;

		public void LogMessage(Log log)
		{
			try
			{
				using (LogContext context = new LogContext(options))
				{
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
			}
			catch (Exception e)
			{

				throw;
			}
		}

	}
}
