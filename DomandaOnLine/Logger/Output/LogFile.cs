using Logger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logger.Output
{
	public class LogFileConfiguration
	{
		
	}
	public class LogFile : ILogOutput
	{
		private const string DEFAULT_FILENAME_TEMPLATE = "log_@Date@.log";

		public string[] paths;
		public string[] filenameTemplates;
		public string template;
		public string application;
		public int daysBeforeDelete;

		public bool AlwaysLog => false;
		public bool ResetTime => true;

		public bool LogPreviousExceptions => true;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="fileNameTemplate">
		///		Template del nome file. Possono essereutilizzati i seguenti tag:
		///		@Level@ Livello d'errore ("Debug,Info,Warning,Error,Fatal")
		///		@Application@ Nome dell'applicazione che ha generato il log
		///		@Date@ Data (in AAAAMMGG) dell'evento di log
		///		Se omesso il tag @Date@ , verrà aggiunto in coda al nome del file.
		///		Se omessa l'estenzione del file verrà messo il valore di default ".log"
		///		A.E "@Application@_@Level_@Date.txt" creerà un file del tipo "Test_Warning_20200101.txt"
		///		Se omesso, il valore di default è "log_@Date@.log"
		/// </param>
		/// <param name="templateText"></param>
		/// <param name="daysBeforeDelete">
		///		Numero di giorni in cui i file vengono mantenuti. tutti i file che sono stati creati da più giorni dalla data specificata verranno cancellati.
		///		Se impostato 0 i file non verranno mai cancellati
		///		Il valore di default è 0
		/// </param>
		/// <param name="application"></param>
		/// <example>
		///	
		/// </example>
		public LogFile(string path, string fileNameTemplate, string templateText, int daysBeforeDelete=0,string application=null){
			this.daysBeforeDelete = daysBeforeDelete;
			this.application = application;
			List<int> pathIndexes = new List<int>();
			foreach (int level in Enum.GetValues(typeof(LogLevel.Levels)))
			{
				pathIndexes.Add(level);
			}
			paths = new string[pathIndexes.Max()+1];
			filenameTemplates = new string[pathIndexes.Max()+1];
			
			pathIndexes.ForEach(i=> {
				SetPathForLevel((LogLevel.Levels)Enum.ToObject(typeof(LogLevel.Levels), i), path);
				SetFileNameTemplateForLevel((LogLevel.Levels)Enum.ToObject(typeof(LogLevel.Levels), i), fileNameTemplate);

			});
		}
		public void SetFileNameTemplateForLevel(LogLevel.Levels level, string template)
		{
			filenameTemplates[(int)level] = template?? DEFAULT_FILENAME_TEMPLATE;
		}


		public void SetPathForLevel(LogLevel.Levels level, string path)
		{
			if (paths.Length<(int)level)
			{
				throw new Exception($"livello di log {level} inesistente");
			}
			if (string.IsNullOrEmpty(path))
			{
				throw new Exception("Il path è obbligatorio");
			}
			try
			{
				bool pathExists = Directory.Exists(path);
				if (!pathExists)
					Directory.CreateDirectory(path);
			}
			catch (Exception ex)
			{
				throw new Exception($"Errore nell'accesso al percorso {path}", ex);
			}
			paths[(int)level] = path;
		}

		public void LogMessage(Log log)
		{
			string path = paths[(int)log.IdLevel];
			string filename = filenameTemplates[(int)log.IdLevel] ??"";
			string durata = "";
			if (log.StartTime.HasValue)
			{
				int? millis;
				millis = (int)(DateTime.Now- log.StartTime.Value).TotalMilliseconds;
				durata = $" ({millis} ms)";
			}
			filename = filename.Replace("@Level@", Enum.GetName(typeof(LogLevel.Levels),log.IdLevel));
			filename = filename.Replace("@Application@", application);
			string filenameTemplate = filename;
			if (!filename.Contains("@Date@"))
			{
				filename +=DateTime.Today.ToString("yyyyMMdd");
			}
			else
			{
				filename = filename.Replace("@Date@", DateTime.Today.ToString("yyyyMMdd"));
			}
			if (!filename.Contains("."))
			{
				filename += ".log";
			}
			string fullPath = $"{path}\\{filename}";

			if (daysBeforeDelete>0)
			{
				DirectoryInfo dir = new DirectoryInfo(path);
				foreach (FileInfo file in dir.GetFiles(filenameTemplate.Replace("@Date@","????????")))
				{
					if (file.LastWriteTime<DateTime.Now.AddDays(-daysBeforeDelete))
					{
						File.Delete(file.FullName);
					}
				}
			}
			string exception = "";
			if (log.Exception!=null)
			{
				exception = $" - {log.Exception.Message} {log.Exception.InnerException?.Message} {log.Exception.StackTrace}";
			}
			string method = "";
			if (!string.IsNullOrEmpty(log.Action))
			{
				method = $" ({log.Action})";
			}
			string ipAddress = "";
			if (!string.IsNullOrEmpty(log.IpAddress))
			{
				ipAddress = $" - ({log.IpAddress})";
			}
			string user = "";
			if (!string.IsNullOrEmpty(log.Username))
			{
				user = $" - ({log.Username})";
			}
			string level = $"{Enum.GetName(typeof(LogLevel.Levels), log.IdLevel)}";
			using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, true))
			using (StreamWriter sw = new StreamWriter(stream))
			{
				sw.WriteLineAsync($"{level}{method}:{user}{ipAddress} - {DateTime.Now}{durata} - {log.Message}{exception}");
			}
		}


	}
}
