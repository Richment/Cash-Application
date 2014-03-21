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
		partial void Geliefert_CanExecute(ref bool result)
		{
			if (this.Rechnungen.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.Rechnungen.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.InRechnung;
		}

		partial void Geliefert_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}



		partial void Versendet_CanExecute(ref bool result)
		{
			if (this.Rechnungen.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.Rechnungen.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Bearbeitet;
		}

		partial void Versendet_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}




		partial void Bezahlt_CanExecute(ref bool result)
		{
			if (this.Rechnungen.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.Rechnungen.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Geliefert;
		}

		partial void Bezahlt_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}
	}
}
