using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class MailSettings
	{
		partial void MailSettings_Created()
		{
			Port = 25;
			SmtpServer = "smtp.domain.com";
			Username = "user@domain.com";
			SenderAddress = "noreply@domain.com";
			Password = "";
		}
	};
}
