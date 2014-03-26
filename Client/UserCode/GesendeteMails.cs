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
	public partial class GesendeteMails
	{
		partial void Resend_CanExecute(ref bool result)
		{
			result = OutgoingMailSet.SelectedItem != null;
		}

		partial void Resend_Execute()
		{
			OutgoingMailSet.SelectedItem.Sended = DateTime.Now;
			Save();
			Refresh();
		}
	}
}
