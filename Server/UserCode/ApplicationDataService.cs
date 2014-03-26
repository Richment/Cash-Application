﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Security.Server;
using System.Net.Mail;
using System.Net;

namespace LightSwitchApplication
{
	public partial class ApplicationDataService
	{
		private const string SENDER = "";
		private const string HOST = "";
		private const string USER = "";
		private const string PASS = "";
		private const int PORT = 25;

		private static void SendEmail(OutgoingMail entity)
		{
			using (SmtpClient client = new SmtpClient(HOST, PORT))
			{
				try
				{
					client.UseDefaultCredentials = false;
					client.Credentials = new NetworkCredential(USER, PASS);
					MailMessage message = new MailMessage(SENDER, entity.Recipient, entity.Subject, entity.Body);
					message.IsBodyHtml = false;
					client.Send(message);
					entity.Result = "Ok";
				}
				catch (Exception ex)
				{
					entity.Result = ex.Message;
				}
			}
		}

		partial void OutgoingMailSet_Inserting(OutgoingMail entity)
		{
			entity.Sended = DateTime.Now;
			SendEmail(entity);
		}

		partial void OutgoingMailSet_Updating(OutgoingMail entity)
		{
			entity.Sended = DateTime.Now;
			SendEmail(entity);
		}

	}
}
