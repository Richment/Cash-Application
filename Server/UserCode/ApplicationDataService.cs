using System;
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

		partial void Artikelliste_Inserting(ArtikellisteItem entity)
		{
			entity.PositionIntern = entity.Position;
		}
		partial void Artikelliste_Updating(ArtikellisteItem entity)
		{ 
			entity.PositionIntern = entity.Position;
		}

		partial void OutgoingMailSet_Inserted(OutgoingMail entity)
		{
			entity.Sended = DateTime.Now;
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
	}
}
