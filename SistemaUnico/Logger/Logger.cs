using Logger.Data;
using Logger.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logger
{
	/// <summary>
	/// Classe per la gestione dei log.
	/// Va inizializzata:
	/// </summary>
	public class Logger
	{
		/// <summary>
		/// Lista degli outputs dei log. Gli outputs sono definiti tramita le interfacce IlogOutput.
		/// A seguito della richiesta di un log verranno invocati gli outputs dal primo all'ultimo secondo le regole definite da ogni interfccia.
		/// </summary>
		private static readonly List<ILogOutput> outputs = new List<ILogOutput>();

		/// <summary>
		/// Aggiunge un output alla lista.
		/// A seguito della richiesta di un log verranno invocati gli outputs dal primo all'ultimo secondo le regole definite da ogni interfccia.
		/// </summary>
		/// <param name="output">
		/// Tipo di output. Deve implementare l'interfaccia IlogOutput
		/// </param>
		public static void AddOutput(ILogOutput output)
		{
			outputs.Add(output);
		}

		/// <summary>
		/// Nome dell'applicazione. Va impostata 
		/// </summary>
		private static string applicationName;

		public static void SetApplicationName(string applicationName)
		{
			Logger.applicationName = applicationName;
		}




		/// <summary>
		/// Contiene la data e l'ora in cui è stato inizializzato il log.
		/// La data potrà essere aggiornata tra goni log se impostato a true la propietà ResetTime
		/// </summary>
		private DateTime? startTime;

		/// <summary>
		/// Nome del Controller invocato da assegnare al log (può essere impostato all'inizializzazione del log)
		/// </summary>
		private readonly string controller;

		/// <summary>
		/// Nome dell'action invocata al log (può essere impostato all'inizializzazione del log)
		/// </summary>
		private readonly string action;

		/// <summary>
		/// Nome del Metodo da assegnare al log (può essere impostato all'inizializzazione del log)
		/// </summary>
		private readonly string method;

		/// <summary>
		/// Nome univoco dell'utente (CF) da assegnare al log (può essere impostato all'inizializzazione del log)
		/// </summary>
		private string username;

		/// <summary>
		/// Nome dell'utente da assegnare al log (può essere impostato all'inizializzazione del log)
		/// </summary>
		private string name;

		/// <summary>
		/// Codice Fiscele Ente di riferimento da assegnare al log (può essere impostato all'inizializzazione del log)
		/// </summary>
		private string ente;

		/// <summary>
		/// Imposta l'utente al log
		/// </summary>
		/// <param name="username">nome utente</param>
		public void SetUsername(string username, string name = null)
		{
			this.username = username;
			this.name = name;
		}

		/// <summary>
		/// Imposta l'utente al log
		/// </summary>
		/// <param name="username">nome utente</param>
		public void SetEnte(string ente)
		{
			this.ente = ente;
		}

		/// <summary>
		/// Identificativo utente da assegnare al log (l'identificativo utente può essere impostato alla creazione dell'oggetto)
		/// </summary>
		private readonly int? idUser;

		/// <summary>
		/// Identificativo IP dell'utente da assegnare al log (l'IP può essere impostato alla creazione dell'oggetto)
		/// </summary>
		private readonly string ipAddress;

		private readonly string applicationIpAddress;

		private readonly string applicationHost;

		private readonly string sessionId;

		public Logger(
			string controller,
			string action,
			string method,
			string username = null,
			string name = null,
			string ente = null,
			int? idUser = null,
			DateTime? startTime = null,
			string ipAddress = null,
			string applicationIpAddress = null,
			string applicationHost = null,
			string sessionId = null
			)
		{
			this.startTime = startTime ?? DateTime.Now;
			this.controller = controller;
			this.action = action;
			this.method = method;
			this.idUser = idUser;
			this.username = username;
			this.name = name;
			this.ente = ente;
			this.ipAddress = ipAddress;
			this.applicationIpAddress = applicationIpAddress;
			this.applicationHost = applicationHost;
			this.sessionId = sessionId;
		}

		private void LogMessage(Log log)
		{
			log.Controller = controller;
			log.Action = action;
			log.Method = method;
			log.IdUser = idUser;
			log.Username = username;
			log.Name = name;
			log.CodiceFiscaleEnte = ente;
			log.StartTime = startTime;
			log.ApplicationName = applicationName;
			log.IpAddress = ipAddress;
			log.ApplicationIpAddress = applicationIpAddress;
			log.ApplicationHost = applicationHost;
			log.SessionId = sessionId;

			bool logged = false;
			Exception previousException = null;
			foreach (ILogOutput logOutput in outputs)
			{
				if (logOutput.LogPreviousExceptions && previousException != null)
				{
					logOutput.LogMessage(new Log()
					{
						IdLevel = LogLevel.ERROR,
						Message = "Errore nella scrittura del log",
						Exception = previousException,
					});
				}
				if (logged && !logOutput.AlwaysLog)
				{
					continue;
				}
				try
				{
					logOutput.LogMessage(log);
					logged = true;
				}
				catch (Exception e)
				{
					previousException = e;
					logged = false;
				}
				if (logOutput.ResetTime) startTime = DateTime.Now;
			}
		}

		public void Trace(string message, int? idEventType = null, object parameters = null, Entity entity = null)
		{
			Task.Factory.StartNew(() =>
			{
				LogMessage(new Log()
				{
					IdLevel = LogLevel.TRACE,
					Message = message,
					EntityId = entity?.Id,
					EntityName = entity?.Name,
					IdEventType = idEventType,
					Parameter = parameters == null ? null : JsonConvert.SerializeObject(parameters),
				});
			});
		}

		public void Information(int idEventType, string message = null, object parameters = null, Entity entity = null)
		{
			Task.Factory.StartNew(() =>
			{
				LogMessage(new Log()
				{
					IdLevel = LogLevel.INFORMATION,
					Message = message,
					EntityId = entity?.Id,
					EntityName = entity?.Name,
					IdEventType = idEventType,
					Parameter = parameters == null ? null : JsonConvert.SerializeObject(parameters),
				});
			});
		}

		public void Warning(int idEventType, string message = null, object parameters = null, Entity entity = null)
		{
			Task.Factory.StartNew(() =>
			{
				LogMessage(new Log()
				{
					IdLevel = LogLevel.WARNING,
					Message = message,
					EntityId = entity?.Id,
					EntityName = entity?.Name,
					IdEventType = idEventType,
					Parameter = parameters == null ? null : JsonConvert.SerializeObject(parameters)
				});
			});
		}

		public void Error(int idEventType, string message = null, Exception exception = null, object parameters = null, Entity entity = null)
		{
			Task.Factory.StartNew(() =>
			{
				LogMessage(new Log()
				{
					IdLevel = LogLevel.ERROR,
					Message = message,
					EntityId = entity?.Id,
					EntityName = entity?.Name,
					IdEventType = idEventType,
					Exception = exception,
					Parameter = parameters == null ? null : JsonConvert.SerializeObject(parameters)
				});
			});
		}

		public void Critical(int idEventType, string message = null, Exception exception = null, object parameters = null, Entity entity = null)
		{
			Task.Factory.StartNew(() =>
			{
				LogMessage(new Log()
				{
					IdLevel = LogLevel.CRITICAL,
					Message = message,
					EntityId = entity?.Id,
					EntityName = entity?.Name,
					IdEventType = idEventType,
					Exception = exception,
					Parameter = parameters == null ? null : JsonConvert.SerializeObject(parameters)
				});
			});
		}
	}
}
