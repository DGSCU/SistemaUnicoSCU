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
	public class LogWS : ILogOutput
	{
		private readonly string logEndpoint;
		private readonly string tokenEndpoint;
		private readonly string username;
		private readonly string password;
		private string token;

		public class LogWSConfig
		{
			public string Endpoint { get; set; }
			public string TokenEndpoint { get; set; }
			public string TokenUsername { get; set; }
			public string TokenPassword { get; set; }
			public bool? AlwaysLog { get; set; }
		}
		public LogWS(LogWSConfig config)
		{
			logEndpoint = config.Endpoint;
			tokenEndpoint = config.TokenEndpoint;
			username = config.TokenUsername;
			password = config.TokenPassword;
			AlwaysLog = config.AlwaysLog ?? false;
		}
		public LogWS(string logEndpoint, string tokenEndpoint, string username, string password, bool alwaysLog = false) : this(
			new LogWSConfig()
			{
				Endpoint = logEndpoint,
				TokenEndpoint = tokenEndpoint,
				TokenUsername = username,
				TokenPassword = password,
				AlwaysLog = alwaysLog
			})
		{
		
		}
		public bool AlwaysLog { get; }
		public bool ResetTime => false;

		public void LogMessage(Log log)
		{
			using HttpClientHandler handler= new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
			using (HttpClient client = new HttpClient(handler))
			{

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
				var content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
				var response = client.PutAsync(this.logEndpoint,content).Result;
				if (!response.IsSuccessStatusCode)
				{
					switch (response.StatusCode)
					{
						default:
							throw new HttpRequestException($"Errore nell'interrogazione al WS : {response.StatusCode} - {response.ReasonPhrase}");
					}
				}
				//var jsonresponse = response.Content.ReadAsStringAsync().Result;
			}
		}

		private string Token
		{
			get
			{
				if (token == null)
				{
					token = GetToken();
				}
				var tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);
				if (jwtToken.ValidTo < DateTime.UtcNow.AddSeconds(+5))//Aggiunti 5 secondi per sicurezza
				{
					token = GetToken();
				}
				return token;
			}
		}

		public bool LogPreviousExceptions => false;

		public string GetToken()
		{
			using HttpClientHandler handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
			using HttpClient client = new HttpClient(handler);
			var content = new StringContent(JsonConvert.SerializeObject(new { Username = this.username, Password = this.password }), Encoding.UTF8, "application/json");
			var response = client.PostAsync(this.tokenEndpoint, content).Result;
			var jsonresponse = response.Content.ReadAsStringAsync().Result;
			return jsonresponse;

		}

	}
}
