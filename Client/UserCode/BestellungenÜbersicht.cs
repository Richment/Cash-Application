using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;

namespace LightSwitchApplication
{
	public partial class BestellungenÜbersicht
	{
		private const string FRMArtikel = "ModalNewItem";
		private const string TXTHeader = "HeaderText";		   
	
		private ArtikellisteItem newItem;
		private string header;

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
			foreach (ArtikellisteItem item in this.DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<ArtikellisteItem>())
				item.Details.DiscardChanges();
			newItem = null;
		}

		partial void BestellungenÜbersicht_Saved()
		{
			Refresh();
		}

		partial void ArtikellisteCollectionAddAndEditNew1_Execute()
		{
			header = "Rechnungsposition hinzufügen...";
			newItem = this.ArtikellisteCollection.AddNew();
			newItem.Rabatt = this.RechnungenSet.SelectedItem.Kunde.Rabatt;
			this.OpenModalWindow();
		}

		partial void ArtikellisteCollectionEditSelected1_CanExecute(ref bool result)
		{
			result = ArtikellisteCollection.SelectedItem != null;
		}

		partial void ArtikellisteCollectionEditSelected1_Execute()
		{
			header = "Rechnungsposition bearbeiten...";
			newItem = this.ArtikellisteCollection.SelectedItem;
			this.OpenModalWindow();
		}

	}
}
