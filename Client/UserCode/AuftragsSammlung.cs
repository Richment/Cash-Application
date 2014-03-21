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
	public partial class AuftragsSammlung
	{
		partial void ProcessList_CanExecute(ref bool result)
		{
			result = this.AuftragsSammlung1.Count > 0;
		}

		partial void ProcessList_Execute()
		{

			this.Refresh();
		}
	}
}
