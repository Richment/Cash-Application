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
	public partial class ArtikelNeu
	{
		partial void ArtikelNeu_InitializeDataWorkspace(List<IDataService> saveChangesTo)
		{
			// Erstellen Sie hier Ihren Code.
			this.ArtikelstammProperty = new ArtikelstammItem();
		}

		partial void ArtikelNeu_Saved()
		{
			// Erstellen Sie hier Ihren Code.
			this.Close(false);
			Application.Current.ShowDefaultScreen(this.ArtikelstammProperty);
		}
	}
}