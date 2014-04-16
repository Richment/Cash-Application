using System.Collections.Generic;
using Microsoft.LightSwitch;

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