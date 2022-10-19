using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace Polling
{
	class Program
	{
		public class ThreadWork
		{
			public static void DoWork()
			{
				try
				{
					HttpClient client = new HttpClient();
					string host = File.ReadAllText("host.txt");
					HttpResponseMessage response = client.GetAsync(host).Result;
					response.EnsureSuccessStatusCode();
					string responseBody = response.Content.ReadAsStringAsync().Result;
					using (FileStream stream = new FileStream("Log.txt", FileMode.Append, FileAccess.Write, FileShare.Write, 4096, true))
					using (StreamWriter sw = new StreamWriter(stream))
					{
						sw.WriteLineAsync(responseBody);
					}
				}
				catch (Exception e)
				{
					using (FileStream stream = new FileStream("Log.txt", FileMode.Append, FileAccess.Write, FileShare.Write, 4096, true))
					using (StreamWriter sw = new StreamWriter(stream))
					{
						sw.WriteLineAsync($"Errore: {e.Message}");
					}
				}
			}
		}

		static void Main(string[] args)
		{
			Thread thread = new Thread(ThreadWork.DoWork);
			thread.Start();
		}
	}
}
