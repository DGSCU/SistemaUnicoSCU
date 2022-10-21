using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class Role
	{
		public const string INTERNO = "Interno";
		public const string AMMINISTRATORE = "Amministratore";
		public const string OPERATORE = "Operatore";
		public const string UTENTE_SPID = "UtenteSPID";
		public const string UTENTE_CREDENZIALI = "UtenteCredenziali";
		public static string[] RuoliInterni = {AMMINISTRATORE, OPERATORE};
		public static string[] Ruoli = { INTERNO, AMMINISTRATORE, OPERATORE, UTENTE_SPID, UTENTE_CREDENZIALI };
	}
}