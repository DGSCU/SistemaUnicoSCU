using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers
{
	public class Utils
	{
		public static void RimuoviSpazi(object oggetto)
		{
			foreach (PropertyInfo proprieta in oggetto.GetType().GetProperties())
			{
				if (proprieta.CanWrite && proprieta.PropertyType == typeof(string))
				{
					string value = (proprieta.GetValue(oggetto) as string);
					if (value != null)
					{
						proprieta.SetValue(oggetto, value.Trim());
					}
				}
			}
		}
	}
}
