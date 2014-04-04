using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;

namespace LightSwitchApplication
{
	public partial class EMailSettings
	{
		partial void EMailSettings_Created()
		{
			if (MailSettingsSet.Count == 0)
			{
				MailSettingsSet.SelectedItem = MailSettingsSet.AddNew();
			}
			else
			{
				MailSettingsSet.SelectedItem = MailSettingsSet.First();
			}
		}
	   
		partial void EMailSettings_Saved()
		{
			this.Close(false);
		}
	}
}