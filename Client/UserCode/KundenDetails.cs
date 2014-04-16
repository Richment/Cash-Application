﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;

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

		partial void ViewDocument_CanExecute(ref bool result)
		{
			result = DokumentePerKunde.SelectedItem != null;
		}

		partial void ViewDocument_Execute()
		{
			string temp = Helper.GetFreeTempFilename("pdf");
			File.WriteAllBytes(temp, DokumentePerKunde.SelectedItem.GeneratedDocument.Bytes);
			Helper.ShellExecute(temp);
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
	}
}