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
using System.Collections.Specialized;

namespace LightSwitchApplication
{
	public partial class RechnungenSetListDetail
	{
		private const string FRMArtikel = "ModalNewItem";
		private const string TXTHeader = "HeaderText";		   
	
		private Artikelliste newItem;
		private string header;

		partial void ArtikellisteCollectionAddAndEditNew_Execute()
		{
			header = "Rechnungsposition hinzufügen...";
			newItem = this.ArtikellisteCollection.AddNew();
			newItem.Rabatt = this.RechnungenSet.SelectedItem.Kunde.Rabatt;
			this.OpenModalWindow();
		}

		partial void ArtikellisteCollectionEditSelected_Execute()
		{
			header = "Rechnungsposition bearbeiten...";
			newItem = this.ArtikellisteCollection.SelectedItem;
			this.OpenModalWindow();
		}
	
		partial void AcceptArtikel_Execute()
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
			foreach (Artikelliste item in this.DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<Artikelliste>())
				item.Details.DiscardChanges();
			newItem = null;
		}

	}
}
