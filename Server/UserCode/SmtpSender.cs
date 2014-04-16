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
		private class SmtpSettings
		{
			public string Host
			{
				get;
				private set;
			}
			public string User
			{
				get;
				private set;
			}
			public string Pass
			{
				get;
				private set;
			}
			public string Sender
			{
				get;
				private set;
			}
			public int Port
			{
				get;
				private set;
			}
			public SmtpSettings(string host, string user, string pass, string sender, int port)
			{
				Host = host;
				User = user;
				Pass = pass;
				Port = port;
			}
		};

		private static SmtpSettings currentSettings;

		internal static void SendEmail(OutgoingMail entity, byte[] attachment = null, string attachmentName = null)
		{
			var settings = GetSettings();
			if (settings == null)
			{
				entity.Result = "Keine gültigen SMTP-Daten.";
				return;
			}

			using (SmtpClient client = new SmtpClient(settings.Host, settings.Port))
			{
				try
				{
					client.UseDefaultCredentials = false;
					client.Credentials = new NetworkCredential(settings.User, settings.Pass);
					MailMessage message = new MailMessage(settings.Sender, entity.Recipient, entity.Subject, entity.Body);
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
		
		internal static void UpdateSettings()
		{
			currentSettings = null;
		}

		private static SmtpSettings GetSettings()
		{
			if (currentSettings == null)
			{
				using (var dw = Application.Current.CreateDataWorkspace())
				{
					MailSettings settings = dw.ApplicationData.MailSettingsSet.FirstOrDefault();
					if (settings == null)
						return null;
					currentSettings = new SmtpSettings(settings.SmtpServer, settings.Username, settings.Password, settings.SenderAddress, settings.Port);
				}
			}
			return currentSettings;
		}
	};
}