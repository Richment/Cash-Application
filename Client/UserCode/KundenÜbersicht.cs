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
	public partial class KundenÜbersicht
	{
		partial void KundenSetEditSelected_CanExecute(ref bool result)
		{
			result = KundenSet.SelectedItem != null;
		}

		partial void KundenSetEditSelected_Execute()
		{
			Application.ShowKundenDetails(KundenSet.SelectedItem.Id);
		}
	}
}
