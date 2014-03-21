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
    public partial class CreateNewRechnungen
    {
		private const string FRMMain = "ModalMain";	
		//private const string FRMArtikel = "ModalNewItem";
		private const string TXTHeader = "HeaderText";
		
		private string header;
		private Rechnungen newItem;


		partial void CreateNewRechnungen_InitializeDataWorkspace(global::System.Collections.Generic.List<global::Microsoft.LightSwitch.IDataService> saveChangesTo)
        {
            this.RechnungenProperty = new Rechnungen();
        }

        partial void CreateNewRechnungen_Saved()
        {
            this.Close(false);
            //Application.Current.ShowDefaultScreen(this.RechnungenProperty);
        }

		partial void CreateNewRechnungen_Activated()
		{
			header = "Rechnungsposition hinzufügen...";
			newItem = this.RechnungenProperty;
			this.OpenModalWindow();
		}

		private void OnModalAvailable(object sender, ControlAvailableEventArgs e)
		{
			ChildWindow win = e.Control as ChildWindow;
			this.FindControl(FRMMain).ControlAvailable -= OnModalAvailable;
			//this.FindControl(TXTHeader).SetProperty("Text", header);
			win.Closed += new EventHandler(OnModalClosed);
		}

		private void OnModalClosed(object sender, EventArgs e)
		{
			ChildWindow win = sender as ChildWindow;
			win.Closed -= OnModalClosed;

			if (!win.DialogResult.HasValue)
				CancelChanges();
			else
				this.Save();

			this.CloseModalWindow();
		}

		private void OpenModalWindow()
		{
			this.OpenModalWindow(FRMMain);
			this.FindControl(FRMMain).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(OnModalAvailable);
		}

		private void CloseModalWindow()
		{
			this.CloseModalWindow(FRMMain);
			this.FindControl(FRMMain).IsVisible = false;
		}

		private void CancelChanges()
		{
			foreach (Rechnungen item in this.DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<Rechnungen>())
				item.Details.DiscardChanges();
			newItem = null;
		}
    }
}