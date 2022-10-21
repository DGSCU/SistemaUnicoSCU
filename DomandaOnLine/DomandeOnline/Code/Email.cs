using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace DomandeOnline.Code
{
	public class Email
	{
		public static void InviaEmail(string to,string testo)
		{
			string from = "s.curti@logicainformatica.it";
			MailMessage message = new MailMessage(from, to);
			message.Subject = "Invio Credenziali";
			SmtpClient client = new SmtpClient("smtps.aruba.it");
			client.Port = 465;
			client.Credentials= new System.Net.NetworkCredential("s.curti@logicainformatica.it", "Radeon9700Logica");
			client.EnableSsl = true;
			client.UseDefaultCredentials = false;
			message.Body = testo;

			try
			{
				client.Send(message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
							ex.ToString());
			}
		}
	}
}