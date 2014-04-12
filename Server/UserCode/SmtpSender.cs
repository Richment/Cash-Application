namespace LightSwitchApplication
{
	using System;
	using System.IO;
	using System.Net;
	using System.Net.Mail;
	using Microsoft.LightSwitch;

	// private static string SENDER = "order@cast4art.de";
	// private static string HOST = "smtp.1und1.de";
	// private static string USER = "order@cast4art.de";
	// private static string PASS = "!order2014!";
	// private static int PORT = 25;				

	internal static class SmtpSender
	{
		internal static void SendEmail(OutgoingMail entity, byte[] attachment = null, string attachmentName = null)
		{
			string host, user, pass, sender;
			int port;
			using (var dw = Application.Current.CreateDataWorkspace())
			{
				MailSettings settings = dw.ApplicationData.MailSettingsSet.FirstOrDefault();
				if (settings == null)
				{
					entity.Result = "Keine gültigen SMTP-Daten.";
					return;
				}
				host = settings.SmtpServer;
				user = settings.Username;
				pass = settings.Password;
				sender = settings.SenderAddress;
				port = settings.Port;
			}

			using (SmtpClient client = new SmtpClient(host, port))
			{
				try
				{
					client.UseDefaultCredentials = false;
					client.Credentials = new NetworkCredential(user, pass);
					MailMessage message = new MailMessage(sender, entity.Recipient, entity.Subject, entity.Body);
					message.IsBodyHtml = false;

					if ((attachment != null) && !String.IsNullOrWhiteSpace(attachmentName))
						using (MemoryStream ms = new MemoryStream(attachment))
							message.Attachments.Add(new Attachment(ms, attachmentName));

					client.Send(message);
					entity.Result = "Ok";
				}
				catch (Exception ex)
				{
					entity.Result = ex.Message;
				}
			}
		}
	};
}