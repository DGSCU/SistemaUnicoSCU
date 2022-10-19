namespace Logger.Data
{
	/// <summary>
	/// Livello di debug
	/// </summary>
	public class LogLevel
	{
		/// <summary>
		/// Log con le informazioni più dettagliate. Il messaggio può contenere informazioni sensibili ed è disabilitato per default
		/// </summary>
		public const int TRACE = 1;
		/// <summary>
		/// Log con le informazioni sul flusso regolare dell'applicativo.
		/// </summary>
		public const int INFORMATION = 2;
		/// <summary>
		/// Log con le informazioni sul flusso non regolare dell'applicativo.
		/// Questo tipo di messaggi generalmente sono relativi a input con dati errati o operazioni non consentite
		/// </summary>
		public const int WARNING = 3;
		/// <summary>
		/// Log con le informazioni sul flusso errato e gestito dell'applicativo.
		/// Questo tipo di messaggi generalmente sono relativi a operazioni non previste o a eccezioni appliative
		/// </summary>
		public const int ERROR = 4;
		/// <summary>
		/// Log con le informazioni sul flusso non previsto dell'applicativo.
		/// Questo tipo di messaggi generalmente sono relativi a eccezioni non gestite dal sistema
		/// </summary>
		public const int CRITICAL = 5;


		public enum Levels{
			/// <summary>
			/// Log con le informazioni più dettagliate. Il messaggio può contenere informazioni sensibili ed è disabilitato per default
			/// </summary>
			Debug = TRACE,
			/// <summary>
			/// Log con le informazioni sul flusso regolare dell'applicativo.
			/// </summary>
			Information = INFORMATION,
			/// <summary>
			/// Log con le informazioni sul flusso non regolare dell'applicativo.
			/// Questo tipo di messaggi generalmente sono relativi a input con dati errati o operazioni non consentite
			/// </summary>
			Warning = WARNING,
			/// <summary>
			/// Log con le informazioni sul flusso errato e gestito dell'applicativo.
			/// Questo tipo di messaggi generalmente sono relativi a operazioni non previste o a eccezioni appliative
			/// </summary>
			Error = ERROR,
			/// <summary>
			/// Log con le informazioni sul flusso non previsto dell'applicativo.
			/// Questo tipo di messaggi generalmente sono relativi a eccezioni non gestite dal sistema
			/// </summary>
			Critical = CRITICAL

		}

		/// <summary>
		/// Identificativo Del livello
		/// </summary>
		/// 
		public int Id { get; set; }
		/// <summary>
		/// Nome del livello di Debug
		/// </summary>
		public string Name { get; set; }
	}
}
