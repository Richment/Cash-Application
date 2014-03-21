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
		#region Stornieren
		partial void Stornieren_CanExecute(ref bool result)
		{
			result = this.InBearbeitung.SelectedItem != null;
		}
		partial void Stornieren_Execute()
		{
			var result = this.ShowMessageBox("Wollen Sie die aktuelle Bestellung wirklich stornieren?", "Warnung", MessageBoxOption.YesNo);
			if(result == System.Windows.MessageBoxResult.Yes)
				this.InBearbeitung.DeleteSelected();
		}
		#endregion

		partial void Geliefert_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.InRechnung;
		}

		partial void Geliefert_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}



		partial void Versendet_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Bearbeitet;
		}

		partial void Versendet_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}




		partial void Bezahlt_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Geliefert;
		}

		partial void Bezahlt_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}

		partial void NextAction_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.InBearbeitung.SelectedItem.Status < (int)LightSwitchApplication.Bestellstatus.Bezahlt;
		}

		partial void Start_Saved()
		{
			// Erstellen Sie hier Ihren Code.
			this.Refresh();
		}

		partial void Editieren_CanExecute(ref bool result)
		{
			result = this.InBearbeitung.SelectedItem != null;
		}

		partial void Editieren_Execute()
		{
			this.Application.ShowBestellungDetails(InBearbeitung.SelectedItem.Id);
		}

	}
}
