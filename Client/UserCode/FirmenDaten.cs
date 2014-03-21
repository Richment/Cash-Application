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
	public partial class FirmenDaten
	{
		partial void FirmenDaten_Saved()
		{
			this.Refresh();
		}

		partial void FirmenDaten_Created()
		{
			if (FirmendatenListe.Count == 0)
			{
				FirmendatenListe.SelectedItem = FirmendatenListe.AddNew();
			}
			else
			{
				FirmendatenListe.SelectedItem = FirmendatenListe.First();
			}
		}
	}
}