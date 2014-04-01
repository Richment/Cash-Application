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
	public partial class Start
	{
		private const string FRM_NEW = "Modal0";
		private const string TXT_NEW = "Text0";
		private const string FRM_NEW_ARTIKEL = "Modal1";
		private const string TXT_NEW_ARTIKEL = "Text1";
		
		private ModalWrapper modNew;
		private Rechnungen current;

		partial void ArtikellisteCollection_Validate(ScreenValidationResultsBuilder results)
		{
			if (this.DataWorkspace.ApplicationData.Details.HasChanges)
			{
				EntityChangeSet changeSet =	this.DataWorkspace.ApplicationData.Details.GetChanges();
			
				foreach (Rechnungen entity in changeSet.AddedEntities.OfType<Rechnungen>())
				{
					if (entity.ArtikellisteCollection.Count() == 0)
					{
						//entity.Details.DiscardChanges(); 
						//results.AddScreenResult("Eine Bestellung muß mindestens eine Position beinhalten.", ValidationSeverity.Warning);
						//results.AddPropertyError("Eine Bestellung muß mindestens eine Position beinhalten.");//, entity.Details.Properties.ArtikellisteCollection);
					}
				} 
				foreach (Rechnungen entity in changeSet.ModifiedEntities.OfType<Rechnungen>())
				{
					if (entity.ArtikellisteCollection.Count() == 0)
					{
						entity.Details.DiscardChanges(); 
						results.AddScreenResult("Eine Bestellung muß mindestens eine Position beinhalten.", ValidationSeverity.Error);
						//results.AddPropertyError("<Fehlermeldung>");
					}
				}
			}
		}


		#region NextAction
		partial void NextAction_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = (this.InBearbeitung.SelectedItem.Status < (int)LightSwitchApplication.Bestellstatus.Bezahlt) && !this.InBearbeitung.SelectedItem.RequiresProcessing;
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
		partial void InBearbeitungEditSelected_CanExecute(ref bool result)
		{
			result = InBearbeitung.SelectedItem != null;
		}
		partial void InBearbeitungEditSelected_Execute()
		{
			this.Application.ShowBestellungDetails(InBearbeitung.SelectedItem.Id);
		}

		partial void Editieren_CanExecute(ref bool result)
		{
			result = this.InBearbeitung.SelectedItem != null;
		}
		partial void Editieren_Execute()
		{
			//InBearbeitung.EditSelected();
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
			result = (this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Bearbeitet) && !this.InBearbeitung.SelectedItem.RequiresProcessing;
		}
		partial void Versand_Execute()
		{
			StartVersendeDialog();
		}
		#endregion
	
		#region Neue Bestellung

		partial void InBearbeitungAddAndEditNew_Execute()
		{
			current = null;
			current = InBearbeitung.AddNew();
			InBearbeitung.SelectedItem = current;
			current.Auftragsnummer = current.GetAuftragsNummer();
			current.RequiresProcessing = true;
			current.Bestelldatum = DateTime.Now;
			current.Status = (int)Bestellstatus.Neu;

			modNew = new ModalWrapper(this, FRM_NEW, TXT_NEW, "Neue Bestellung eingeben...")
			{
				CancelMethod = () =>
				{
					foreach (Rechnungen item in DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<Rechnungen>())
						if (item.Id == current.Id)
						{
							foreach (ArtikellisteItem pos in DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.OfType<ArtikellisteItem>())
								pos.Details.DiscardChanges();
							foreach (ArtikellisteItem pos in DataWorkspace.ApplicationData.Details.GetChanges().ModifiedEntities.OfType<ArtikellisteItem>())
								pos.Details.DiscardChanges();		
							
							item.Details.DiscardChanges();
						}
					current = null;
				},
				ProceedMethod = () =>
				{
					//var b = new ScreenValidationResultsBuilder();
					//this.InBearbeitung_Validate(b);
					 //DataWorkspace.ApplicationData.Details.GetChanges().AddedEntities.ToList().ForEach(n=>n.Details.
//					Application.Details.Dispatcher.BeginInvoke(() => this.Save());
				}
			};

			InBearbeitung.SelectedItem = current;
			modNew.Show();
		}
		
		partial void Ok_NeueRechnung_CanExecute(ref bool result)
		{
			if (current == null)
				result = false;
			else
				result = (current.BezahlartItem != null) && !String.IsNullOrWhiteSpace(current.Auftragsnummer) && (current.ArtikellisteCollection.Count() > 0) && current.ArtikellisteCollection.All(n => (n.ArtikelstammItem != null) && (n.Anzahl > 0));
		}
		partial void Ok_NeueRechnung_Execute()
		{
			modNew.Close();
		}		

		partial void ArtikellisteCollection1AddAndEditNew1_Execute()
		{
			var tmp = ArtikellisteCollection.AddNew();
			 	if (current.Kunde != null)
					tmp.Rabatt = current.Kunde.Rabatt;
				else
					tmp.Rabatt=10;
				
			this.ArtikellisteCollection.SelectedItem = tmp;
			ArtikellisteCollection.EditSelected();
		}
		

		#endregion

		#region Bezahlung

		partial void Bezahlung_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = ((this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Geliefert) || (this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Zahlungsverzug)) && !this.InBearbeitung.SelectedItem.RequiresProcessing;

		}
		partial void Bezahlung_Execute()
		{

		}

		partial void Bezahlt_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = ((this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Geliefert) || (this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.Zahlungsverzug)) && !this.InBearbeitung.SelectedItem.RequiresProcessing;
		}
		partial void Bezahlt_Execute()
		{

		}

		#endregion


		partial void Lieferung_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			result = (this.InBearbeitung.SelectedItem.Status == (int)LightSwitchApplication.Bestellstatus.InRechnung) && !this.InBearbeitung.SelectedItem.RequiresProcessing;
		}
		partial void Lieferung_Execute()
		{
			// Erstellen Sie hier Ihren Code.

		}





		private void StartVersendeDialog()
		{
		
		}





	}
}
