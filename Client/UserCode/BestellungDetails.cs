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
	public partial class BestellungDetails
	{
		private const string FRMArtikel = "ModalNewItem";
		private const string TXTHeader = "HeaderText";

		private ArtikellisteItem newItem;
		private string header;

		
		partial void Rechnungen_Loaded(bool succeeded)
		{
			this.SetDisplayNameFromEntity(this.Rechnungen);
		}

		partial void Rechnungen_Changed()
		{
			this.SetDisplayNameFromEntity(this.Rechnungen);
		}

		partial void BestellungDetails_Saved()
		{
			this.SetDisplayNameFromEntity(this.Rechnungen);
		}


		partial void ArtikellisteCollectionAddAndEditNew_CanExecute(ref bool result)
		{
			if (ArtikellisteCollection == null)
				result = false;
			else
				result = true;	 
		}

		partial void ArtikellisteCollectionAddAndEditNew_Execute()
		{
			header = "Rechnungsposition hinzufügen...";
			newItem = this.ArtikellisteCollection.AddNew();
			newItem.Rabatt = this.Rechnungen.Kunde.Rabatt;
			this.OpenModalWindow();		
		}

		partial void ArtikellisteCollectionEditSelected_CanExecute(ref bool result)
		{
			if (ArtikellisteCollection == null)
				result = false;
			else
				result = ArtikellisteCollection.SelectedItem != null;
		}

		partial void ArtikellisteCollectionEditSelected_Execute()
		{
			header = "Rechnungsposition bearbeiten...";
			newItem = this.ArtikellisteCollection.SelectedItem;
			this.OpenModalWindow();
		}

		partial void AcceptNewItem_Execute()
		{
			this.CloseModalWindow();
		}


		private void OnModalAvailable(object sender, ControlAvailableEventArgs e)
		{
			ChildWindow win = e.Control as ChildWindow;
			this.FindControl(FRMArtikel).ControlAvailable -= OnModalAvailable;
			this.FindControl(TXTHeader).SetProperty("Text", header);
			win.Closed += new EventHandler(OnModalClosed);
		}

		private void OnModalClosed(object sender, EventArgs e)
		{
			ChildWindow win = sender as ChildWindow;
			win.Closed -= OnModalClosed;

			if (!win.DialogResult.HasValue)
				CancelChanges();

			this.CloseModalWindow();
		}

		private void OpenModalWindow()
		{
			this.OpenModalWindow(FRMArtikel);
			this.FindControl(FRMArtikel).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(OnModalAvailable);
		}

		private void CloseModalWindow()
		{
			this.CloseModalWindow(FRMArtikel);
			this.FindControl(FRMArtikel).IsVisible = false;
		}

		private void CancelChanges()
		{
			foreach (ArtikellisteItem item in this.DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<ArtikellisteItem>())
				item.Details.DiscardChanges();
			newItem = null;
		}

	}
}