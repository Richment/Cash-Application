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
	public partial class KundenDetails
	{
		private const string FRMLiefer = "ModalAddressLiefer";
		private const string TXTHeader = "HeaderText";

		private AdressenSetItem newItem;
		private string header;


		partial void Kunden_Loaded(bool succeeded)
		{
			this.SetDisplayNameFromEntity(this.Kunden);
		}

		partial void Kunden_Changed()
		{
			this.SetDisplayNameFromEntity(this.Kunden);
		}

		partial void KundenDetails_Saved()
		{
			Close(false);
		}

		#region LieferAdresse

		private void LieferAvailable(object sender, ControlAvailableEventArgs e)
		{
			this.FindControl(FRMLiefer).ControlAvailable -= LieferAvailable;
			this.FindControl(TXTHeader).SetProperty("Text", header);
			((ChildWindow)e.Control).Closed += new EventHandler(LieferClosed);
		}

		private void LieferClosed(object sender, EventArgs e)
		{
			((ChildWindow)sender).Closed -= LieferClosed;

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

		partial void LieferOK_Execute()
		{
			this.LieferCloseWindow();
		}

		partial void AdressenAddAndEditNew_Execute()
		{
			header = "Lieferadresse hinzufügen...";
			newItem = this.Adressen.AddNew();
			this.OpenModalWindow(FRMLiefer);
			this.FindControl(FRMLiefer).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(LieferAvailable);
		}

		partial void AdressenEditSelected_Execute()
		{
			header = "Lieferadresse bearbeiten...";
			newItem = this.Adressen.SelectedItem;
			this.OpenModalWindow(FRMLiefer);
			this.FindControl(FRMLiefer).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(LieferAvailable);
		}

		#endregion

		partial void Anzeigen_Execute()
		{
			var sel = DocumentsCollection.SelectedItem;
			if (sel == null)
				return;
			
			string path = Helper.GetFreeTempFilename("pdf");
			if (sel.GeneratedDocument != null)
			{
				File.WriteAllBytes(path, sel.GeneratedDocument.Bytes);
				Helper.ShellExecute(path);
			}
		}

		partial void Speichern_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}
	}
}