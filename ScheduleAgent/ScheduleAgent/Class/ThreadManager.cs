using ScheduleAgent.Controllers;
using ScheduleAgent.Models;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading;

namespace ScheduleAgent.Class
{
	public class ThreadManager
	{
		public class JobResponse
		{
			public bool Success { get; set; }
			public string Message { get; set; }
			public List<string> Errors { get; set; }
		}
		public static class ThreadStatus
		{
			public const string Running = "In esecuzione";
			public const string Waiting = "In attesa";
			public const string Terminated = "Terminato";
			public const string Error = "Errore";
		}

		/** Classe per contenere le informazioni di ogni job **/
		public class Execution
		{
			public int IdJob { get; set; }
			public int NumeroEsecuzioni { get; set; }
			public int ErroriConsecutivi { get; set; }
			public DateTime? UltimaEsecuzione { get; set; }
			public DateTime? FineEsecuzione { get; set; }
		}

		public ThreadManager(string guid)
		{
			CodiceThread = guid;
		}

		public void Process()
		{
			//Ciclo di attesa fine esecuzione thread precedente
			while (ApplicationGlobals.Instance.RunningThreadStatus == ThreadStatus.Running)
			{
				Thread.Sleep(500);
			}
			/* Recupero valori configurazione */
			Parameters parametri;
			try
			{
				parametri = new Parameters();
			}
			catch (Exception e)
			{
				Log.Error (e,"Errore nel recupero dei valori di configurazione");
				lock (ApplicationGlobals.Instance)
				{
					ApplicationGlobals.Instance.RunningThreadStatus = ThreadStatus.Error;
					return;
				}
			}
			int cycle = 1;
			//Inizializzazione variabili globali inizio esecuzione thread
			lock (ApplicationGlobals.Instance)
			{
				ApplicationGlobals.Instance.RunningThreadStartTime = DateTime.Now;
				ApplicationGlobals.Instance.RunningThreadStartTimeLastCycle = null;
				ApplicationGlobals.Instance.RunningThreadEndTimeLastCycle = null;
				ApplicationGlobals.Instance.RunningThreadNumberOfExecutions = 0;
			}
			Log.Information("Avviato Schedule Agent");

			ConsecutiveErrors = 0;
			List<Execution> esecuzioni = new List<Execution>();
			//Loop infinito finché non viene variato il Guid (nel caso sia stato rieseguito o annullato)
			while (ApplicationGlobals.Instance.RunningThreadGuid == CodiceThread)
			{
				lock (ApplicationGlobals.Instance)
				{
					ApplicationGlobals.Instance.RunningThreadStatus = ThreadStatus.Running;
					ApplicationGlobals.Instance.RunningThreadNumberOfExecutions = cycle;
					ApplicationGlobals.Instance.RunningThreadStartTimeLastCycle = DateTime.Now;
				}


				int giornoSettimana = (int)DateTime.Today.DayOfWeek;
				List<JobSchedule> jobs;
				using (var context = new HeliosContext())
				{
					jobs = context.JobSchedule
						.Where(x => (x.GiornoSettimana ?? giornoSettimana) == giornoSettimana)
						.Include(x => x.Job)
						.ToList();
				}
				foreach (JobSchedule schedule in jobs)
				{
					using (LogContext.PushProperty("Job", schedule.Job.Nome))
					{
						string nomeJob = schedule.Job.Nome;
						DateTime oraSchedulazione = DateTime.Today.Add(schedule.OraInizio);
						/*** Verifico se è passata l'ora di esecuzione ***/
						if (DateTime.Now < oraSchedulazione)
							continue;
						/*** Verifico che non sia stato già eseguito il job ***/
						using (var context = new HeliosContext())
						{
							if (context.JobExecution
								.Where(x => x.IdJob == schedule.IdJob &&
											x.DataInizioEsecuzione >= oraSchedulazione)
								.Any()
								)
								continue;
						}
						Execution esecuzione = esecuzioni.Where(x => x.IdJob == schedule.IdJob).FirstOrDefault();
						if (esecuzione == null)
						{
							esecuzione = new Execution()
							{
								IdJob = schedule.IdJob,
								NumeroEsecuzioni = 0,
								ErroriConsecutivi = 0
							};
							esecuzioni.Add(esecuzione);
						}
						/* Ciclo per la ri-esecuzione in caso di errori */
						while (true)
						{
							esecuzione.NumeroEsecuzioni++;
							Log.Information($"Inizio Esecuzione Processo {nomeJob} - ciclo n° {esecuzione.NumeroEsecuzioni}");

							/* Registrazione inizio esecuzione */
							JobExecution esecuzioneJob = null;
							try
							{
								using (var context = new HeliosContext())
								{
									esecuzioneJob = new JobExecution()
									{
										IdJob = schedule.IdJob,
										DataInizioEsecuzione = DateTime.Now
									};
									context.JobExecution.Add(esecuzioneJob);
									context.SaveChanges();
								}
								lock (ApplicationGlobals.Instance)
								{
									ApplicationGlobals.Instance.LastJobStartTime = DateTime.Now;
									ApplicationGlobals.Instance.LastJobEndTime = null;
								}
								//* Seleziona Processo da eseguire *//
								switch (schedule.IdJob)
								{
									case Job.DOMANDA_ONLINE:
										var response= JobsController.ProtocollaJob();
										lock (ApplicationGlobals.Instance)
										{
											ApplicationGlobals.Instance.LastExecutionResponse = response;
										}
										if (!response.Success)
										{
											string oggettoEmail = $"Schedule Agent - Errori nell'esecuzione del job {nomeJob}";
											string testoEmail = $"Il job {nomeJob} è terminaco con errori.<br/>{response.Message}<br/>Errori:<br>{string.Join("<br/>",response.Errors)}";
											using (MailMessage message = new MailMessage(parametri.IndirizzoFromMailInvioErrore, parametri.IndirizzoToMailInvioErrore)
											{
												Subject = oggettoEmail,
												Body = testoEmail,
												IsBodyHtml = true
											})
											using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
											{
												UseDefaultCredentials = true
											})
											{
												try
												{
													client.Send(message);
												}
												catch (Exception e)
												{
													string errore = "Errore nell'invio della mail ";
													Log.Error(e, errore);
												}
											}
											break;
										}
										cycle++;
										break;
									default:
										throw new Exception($"Processo {nomeJob} non gestito");
								}
								lock (ApplicationGlobals.Instance)
								{
									ApplicationGlobals.Instance.LastJobEndTime = DateTime.Now;
								}
								using (var context = new HeliosContext())
								{
									esecuzioneJob = context.JobExecution.Where(x => x.Id == esecuzioneJob.Id).SingleOrDefault();
									esecuzioneJob.DataFineEsecuzione = DateTime.Now;
									esecuzioneJob.Esito = "Eseguito";
									context.SaveChanges();
								}
								esecuzione.ErroriConsecutivi = 0;
								Log.Information($"Eseguito ciclo n° {esecuzione.NumeroEsecuzioni} del processo {nomeJob}");
								break;
							}
							catch (Exception e)
							{
								esecuzione.ErroriConsecutivi++;

								Log.Error(e, $"Errore nell'esecuzione del processo {nomeJob}");
								using (var context = new HeliosContext())
								{
									esecuzioneJob = context.JobExecution.Where(x => x.Id == esecuzioneJob.Id).SingleOrDefault();
									esecuzioneJob.DataFineEsecuzione = DateTime.Now;
									esecuzioneJob.Esito = "Errore";
									context.SaveChanges();
								}
								if (esecuzione.ErroriConsecutivi >= MaxNumberOfConsecutiveErrors)
								{
									lock (ApplicationGlobals.Instance)
									{
										ApplicationGlobals.Instance.RunningThreadEndTimeLastCycle = DateTime.Now;
										ApplicationGlobals.Instance.RunningThreadStatus = ThreadStatus.Error;
									}
									Log.Fatal(e, $"Sono stati generati {MaxNumberOfConsecutiveErrors} errori consecutivi. Il processo verrà terminato");
									break;
								}
								else
								{
									Thread.Sleep(10000);
								}

							}
							Log.Information($"Fine Esecuzione Processo {nomeJob}");
						}
					}
				}

				if (ApplicationGlobals.Instance.RunningThreadStatus == ThreadStatus.Running)
				{
					lock (ApplicationGlobals.Instance)
					{
						ApplicationGlobals.Instance.RunningThreadEndTimeLastCycle = DateTime.Now;
						ApplicationGlobals.Instance.RunningThreadStatus = ThreadStatus.Waiting;
						TimeSpan tempoEsecuzione = (ApplicationGlobals.Instance.RunningThreadEndTimeLastCycle.Value - ApplicationGlobals.Instance.RunningThreadStartTimeLastCycle.Value);

					}
				}
				if (cycle == MaxNumberOfCycles)
				{
					lock (ApplicationGlobals.Instance)
					{
						ApplicationGlobals.Instance.RunningThreadStatus = ThreadStatus.Terminated;
					}
					break;
				}
				if (ApplicationGlobals.Instance.RunningThreadStatus == ThreadStatus.Error)
				{
					string oggettoEmail = $"Schedule Agent - Errore critico";
					string testoEmail = $"Attenzione si è verificato un errore critico ed il servizio di pianificazione dei processi è stato terminato.";
					using (MailMessage message = new MailMessage(parametri.IndirizzoFromMailInvioErrore, parametri.IndirizzoToMailInvioErrore)
					{
						Subject = oggettoEmail,
						Body = testoEmail,
						IsBodyHtml = true
					})
					using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
					{
						UseDefaultCredentials = true
					})
					{
						try
						{
							client.Send(message);
						}
						catch (Exception e)
						{
							string errore = "Errore nell'invio della mail ";
							Log.Error(e, errore);
						}
					}
					break;
				}
				if (ApplicationGlobals.Instance.RunningThreadStatus == ThreadStatus.Waiting)
					Thread.Sleep(SecondsEachCycle);
			}
			Log.Information($"Terminato Schedule Agent");
			if (string.IsNullOrEmpty(ApplicationGlobals.Instance.RunningThreadGuid))
				lock (ApplicationGlobals.Instance)
				{
					ApplicationGlobals.Instance.RunningThreadStatus = ThreadStatus.Terminated;
				}
		}

		private string CodiceThread;
		public int SecondsEachCycle { get{ return 1000 * 60 * 1; }  }
		public int MaxNumberOfCycles { get { return 0; } }
		public int MaxNumberOfConsecutiveErrors { get { return 5; } }
		private int ConsecutiveErrors { get; set; }


	}
}


