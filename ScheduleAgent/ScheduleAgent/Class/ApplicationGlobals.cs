using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ScheduleAgent.Class.ThreadManager;

namespace ScheduleAgent.Class
{
	public class ApplicationGlobals
	{
		private static ApplicationGlobals instance = null;
		private static readonly object padlock = new object();
		private static List<string> runningThreadLog;


		ApplicationGlobals()
		{
			RunningThreadLogSize = 100;
			runningThreadLog = new List<string>();
		}

		public static ApplicationGlobals Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new ApplicationGlobals();
					}
					return instance;
				}
			}
		}
		public JobResponse LastExecutionResponse { get; set; }
		public string EmailDomandeAnnullateTemplate { get; set; }
		public string RunningThreadGuid { get; set; }
		public string RunningThreadStatus { get; set; }
		public DateTime? RunningThreadStartTime { get; set; }
		public int RunningThreadNumberOfExecutions { get; set; }
		public DateTime? RunningThreadStartTimeLastCycle { get; set; }
		public DateTime? RunningThreadEndTimeLastCycle { get; set; }

		public DateTime? LastJobStartTime { get; set; }
		public DateTime? LastJobEndTime { get; set; }
		public int RunningThreadLogSize { get; set; }
		public List<string> RunningThreadLog
		{
			get
			{
				return runningThreadLog;
			}
		}

		public void addRunningThreadLog(string text)
		{
			RunningThreadLog.Insert(0, text);
			if (RunningThreadLog.Count > RunningThreadLogSize)
				RunningThreadLog.RemoveAt(RunningThreadLogSize);
		}

	}
}