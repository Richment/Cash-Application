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
	public partial class AddAddress
	{
		private const string NAME = "frmModal";
		private AdressenSetItem newItem;
		

		private void frmAddAddressAvailable(object sender, ControlAvailableEventArgs e)
		{
			this.FindControl(NAME).ControlAvailable -= frmAddAddressAvailable;
			((ChildWindow)e.Control).Closed += new EventHandler(frmAddAddressClosed);
		}

		private void frmAddAddressClosed(object sender, EventArgs e)
		{
			((ChildWindow)sender).Closed -= frmAddAddressClosed;

			if (newItem != null)
				newItem.Lieferadresse = IsShippingAddress;
			
			if (!((ChildWindow)sender).DialogResult.HasValue)
				CancelfrmAddAddressChanges();

			this.CloseModalWindow();
		}

		private void CancelfrmAddAddressChanges()
		{
			foreach (AdressenSetItem item in this.DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<AdressenSetItem>())
				item.Details.DiscardChanges();

			newItem = null;
		}

		private void CloseModalWindow()
		{
			this.CloseModalWindow(NAME);
			this.FindControl(NAME).IsVisible = false;
		}

		partial void SubmitAdd_Execute()
		{
			this.CloseModalWindow();
		}

	

		partial void gridEditSelected_Execute()
		{
			newItem = this.AdressenSet.SelectedItem;
			this.OpenModalWindow(NAME);
			this.FindControl(NAME).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(frmAddAddressAvailable);
		}

		partial void gridAddAndEditNew_Execute()
		{
			// Erstellen Sie hier Ihren Code.
			newItem = this.AdressenSet.AddNew();
			
			this.OpenModalWindow(NAME);
			this.FindControl(NAME).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(frmAddAddressAvailable);
		}

	}
}
