using Logger.Data;
using System;

namespace Logger.Output
{
	/// <summary>
	/// Interfaccia di output del log
	/// </summary>
	public interface ILogOutput
	{
		/// <summary>
		/// Indica se il log viene sempre registrato indipendentemente dall'esito dell'output precedente.
		/// false = log solo se il precedenta output è fallito ; true = sempte
		/// </summary>
		bool AlwaysLog { get; }

		/// <summary>
		/// Indica se a seguito di un'eccezione del precedente log verrà richiesto di loggare tale eccezione.
		/// </summary>
		bool LogPreviousExceptions { get; }
		
		/// <summary>
		/// Indica se il tempo tra un log e l'altro viene reimpostato
		/// false= il tempo di esecuzione del log viene calcolato dalla generazione del primo log
		/// true = il tempo di esecuzione è calcolato da un log e l'altro.
		/// </summary>
		bool ResetTime { get; }

		/// <summary>
		/// Metodo per la registrazione del log.
		/// </summary>
		/// <param name="log">
		/// Log da registrare
		/// </param>
		void LogMessage(Log log);
	}
}
