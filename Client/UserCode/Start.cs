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
		private const string FRM_NEW = "Modal0";
		private const string TXT_NEW = "Text0";
		private const string FRM_NEW_ARTIKEL = "Modal1";
		private const string TXT_NEW_ARTIKEL = "Text1";
		
		private ModalWrapper mod;
		private Rechnungen current;


		partial void Start_Saved()
		{
			this.Refresh();
		}

		#region NextAction
		partial void NextAction_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.InBearbeitung.SelectedItem.Status < (int)LightSwitchApplication.Bestellstatus.Bezahlt;
		}
		partial void NextAction_Execute()
		{
			var status = this.InBearbeitung.SelectedItem.Status;
			switch (status)
			{
				case (int)Bestellstatus.Bearbeitet:
					StartVersendeDialog();
					break;

				case (int)Bestellstatus.Versendet:

					break;

				case (int)Bestellstatus.InRechnung:

					break;

				case (int)Bestellstatus.Geliefert:

					break;
			}
		}
		#endregion

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
	
		#region Editieren
		partial void Editieren_CanExecute(ref bool result)
		{
			result = this.InBearbeitung.SelectedItem != null;
		}
		partial void Editieren_Execute()
		{
			this.Application.ShowBestellungDetails(InBearbeitung.SelectedItem.Id);
		}
		#endregion

		#region  Versand
		partial void Versand_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Bearbeitet;
		}
		partial void Versand_Execute()
		{
			StartVersendeDialog();
		}
		#endregion
	
		#region Neue Bestellung
		partial void InBearbeitungAddAndEditNew_Execute()
		{
			InBearbeitung.SelectedItem = current = InBearbeitung.AddNew();
			mod = new ModalWrapper(this, FRM_NEW, TXT_NEW, "Neue Bestellung eingeben...")
			{
				CancelMethod = () =>
				{
					foreach (Rechnungen item in DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<Rechnungen>())
						if( item.Id == current.Id)
							item.Details.DiscardChanges();
					current = null;
				},
				ProceedMethod = () =>
				{
					this.Details.Dispatcher.BeginInvoke(ShowArtikelDialog);
				}
			};
			current.Bestelldatum = DateTime.Now;
			current.Status = (int)Bestellstatus.Neu;
			mod.Show();
		}
		partial void Ok_NeueRechnung_CanExecute(ref bool result)
		{
			if (current == null)
				result = false;
			else
				result = (current.BezahlartItem != null) && !String.IsNullOrWhiteSpace(current.Referenznummer) && (current.ArtikellisteCollection.Count() > 0);
		}
		partial void Ok_NeueRechnung_Execute()
		{
			mod.Close();
		}

		public void ShowArtikelDialog()
		{
		//	mod= new ModalWrapper(


		}
		partial void OK_Artikel_CanExecute(ref bool result)
		{
			result = current.ArtikellisteCollection.Count() > 0;
		}
		partial void OK_Artikel_Execute()
		{
			mod.Close();
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

		}




		private void StartVersendeDialog()
		{
		
		}



	
	}
}
