using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
using System.Windows.Controls;

namespace LightSwitchApplication
{
	public partial class KundenÜbersicht
	{
		private const string FRMRechnung = "ModalAddressRechnung";
		private const string FRMLiefer = "ModalAddressLiefer";
		private AdressenSetItem newItem;


		partial void Kunden_Loaded(bool succeeded)
		{
			// Erstellen Sie hier Ihren Code.
			this.SetDisplayNameFromEntity(this.Kunden);
		}

		partial void Kunden_Changed()
		{
			// Erstellen Sie hier Ihren Code.
			this.SetDisplayNameFromEntity(this.Kunden);
		}

		partial void KundenÜbersicht_Saved()
		{
			// Erstellen Sie hier Ihren Code.
			this.SetDisplayNameFromEntity(this.Kunden);
		}






		#region Liefer

		private void LieferAvailable(object sender, ControlAvailableEventArgs e)
		{
			this.FindControl(FRMLiefer).ControlAvailable -= LieferAvailable;
			((ChildWindow)e.Control).Closed += new EventHandler(LieferClosed);
		}

		private void LieferClosed(object sender, EventArgs e)
		{
			((ChildWindow)sender).Closed -= LieferClosed;

			if (newItem != null)
				; //newItem.Lieferadresse = true;

			if (!((ChildWindow)sender).DialogResult.HasValue)
				LieferCancelChanges();

			this.LieferCloseWindow();
		}

		private void LieferCancelChanges()
		{
			foreach (AdressenSetItem item in this.DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<AdressenSetItem>())
				item.Details.DiscardChanges();
			newItem = null;
		}

		private void LieferCloseWindow()
		{
			this.CloseModalWindow(FRMLiefer);
			this.FindControl(FRMLiefer).IsVisible = false;
		}

		#endregion
	
		partial void LieferadressenAddAndEditNew1_Execute()
		{
			newItem = this.Adressen.AddNew();

			this.OpenModalWindow(FRMLiefer);
			this.FindControl(FRMLiefer).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(LieferAvailable);
	   	}

	
		partial void LieferadressenEditSelected_Execute()
		{
			newItem = this.Adressen.SelectedItem;
			this.OpenModalWindow(FRMLiefer);
			this.FindControl(FRMLiefer).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(LieferAvailable);
	  	}

		partial void LieferOK_Execute()
		{
			this.LieferCloseWindow();	
		}
	}
}