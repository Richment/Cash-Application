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
	public partial class Start
	{
		partial void Bearbeiten_CanExecute(ref bool result)
		{
			if (this.Rechnungen.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.Rechnungen.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Neu;
		}

		partial void Bearbeiten_Execute()
		{
			
		}
	}
}
