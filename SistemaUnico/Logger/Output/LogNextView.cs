using Logger.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Logger.Output
{
	public class LogNextView : ILogOutput
	{
		private readonly string logEndpoint;
		private readonly string queueUsername;
		private readonly string queuePassword;
		private readonly string queue;

		public class LogNextViewConfig
		{
			public string Endpoint { get; set; }
			public string Queue { get; set; }
			public string QueueUsername { get; set; }
			public string QueuePassword { get; set; }
			public bool? AlwaysLog { get; set; }
		}
		public LogNextView(LogNextViewConfig config)
		{
			logEndpoint = config.Endpoint;
			queue = config.Queue;
			queueUsername = config.QueueUsername;
			queuePassword = config.QueuePassword;
			AlwaysLog = config.AlwaysLog ?? false;
		}

		public bool ResetTime => false;

		public LogNextView(string logEndpoint,string queue,string queueUsername, string queuePassword) : this(
			new LogNextViewConfig()
			{
				Endpoint = logEndpoint,
				Queue=queue,
				QueueUsername=queueUsername,
				QueuePassword=queuePassword
			})
		{
		
		}
		public bool AlwaysLog { get; }

		public void LogMessage(Log log)
		{
			if (log.IdLevel!=Data.LogLevel.TRACE && log.IdLevel != Data.LogLevel.CRITICAL)
			{
				return;
			}
			using HttpClientHandler handler= new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
			using (HttpClient client = new HttpClient(handler))
			{
				string status = "errore";
				if (log.Exception==null)
					status = "ok";
				string timestart = ((long)log.StartTime.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
				string timeEnd = ((long)log.TimeStamp.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
				var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("queueUsername", queueUsername),
					new KeyValuePair<string, string>("queuePassword", queuePassword),
					new KeyValuePair<string, string>("queue", queue),
					new KeyValuePair<string, string>("ts_start_ms", timestart),
					new KeyValuePair<string, string>("ts_end_ms", timeEnd),
					new KeyValuePair<string, string>("hostName", log.ApplicationHost),
					new KeyValuePair<string, string>("hostAddress", log.ApplicationIpAddress),
					new KeyValuePair<string, string>("service", log.ApplicationName),
					new KeyValuePair<string, string>("user", log.Username??"-"),
					new KeyValuePair<string, string>("userAddress", log.IpAddress),
					new KeyValuePair<string, string>("step", "1"),
					new KeyValuePair<string, string>("userAddress", log.IpAddress),
					new KeyValuePair<string, string>("action", $"{log.Controller}.{log.Action}.{log.Method}"),
					new KeyValuePair<string, string>("status", status ),
					new KeyValuePair<string, string>("errorDescription", log.Exception?.Message),
					new KeyValuePair<string, string>("originalSessionId", log.SessionId),
					new KeyValuePair<string, string>("currentSessionId", log.SessionId)

				});

				var response = client.PostAsync(logEndpoint, formContent).Result;
				if (!response.IsSuccessStatusCode)
				{
					switch (response.StatusCode)
					{
						default:
							throw new HttpRequestException($"Errore nell'interrogazione al WS : {response.StatusCode} - {response.ReasonPhrase}");
					}
				}
			}
		}

		public bool LogPreviousExceptions => false;
	}
}
