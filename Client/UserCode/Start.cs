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
using PixataCustomControls.Presentation.Controls;
using Microsoft.LightSwitch.Threading;
using System.Windows.Threading;

namespace LightSwitchApplication
{
	public partial class Start
	{
		private const string FRM_NEW = "Modal0";
		private const string TXT_NEW = "Text0";
		private const string FRM_VERSENDE = "Modal1";
		private const string TXT_VERSENDE = "Text1";


		private ModalWrapper modal;
		private Rechnungen current;

		partial void Start_Created()
		{
			if (String.IsNullOrWhiteSpace(Application.StartScreenMessage))
			{
				var text = this.FindControl("StatusText");
				if (text != null)
					text.IsVisible = false;
			}
			else
			{
				StatusText = Application.StartScreenMessage;
			}

			var toolbar = this.FindControl("HeaderToolbar");
			if (toolbar != null)
			{
				toolbar.ControlAvailable += (s, e) =>
				{
					StaticToolbar stb = ((StaticToolbar)e.Control);
					stb.ButtonClick += new EventHandler<StaticToolbarEventArgs>(HeaderToolbar_BtnClick);
				};
			}
		}

		private void HeaderToolbar_BtnClick(object sender, StaticToolbarEventArgs e)
		{
			switch (e.ButtonNumber)
			{
				case 1:
					Details.Dispatcher.BeginInvoke(new Action(() => Application.ShowAuftragsSammlung()));
					break;

				case 2:
					Details.Dispatcher.BeginInvoke(new Action(() => Application.ShowKundenÜbersicht()));
					break;

				case 3:
					Details.Dispatcher.BeginInvoke(new Action(() => Application.ShowArtikelÜbersicht()));
					break;

				case 4:
					Details.Dispatcher.BeginInvoke(new Action(() => Application.ShowDokumentenÜbersicht()));
					break;

				case 5:
					Details.Dispatcher.BeginInvoke(new Action(() => Application.ShowGesendeteMails()));
					break;
			}
		}

		partial void ArtikellisteCollection_Validate(ScreenValidationResultsBuilder results)
		{
			if (this.DataWorkspace.ApplicationData.Details.HasChanges)
			{
				EntityChangeSet changeSet = this.DataWorkspace.ApplicationData.Details.GetChanges();

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

		partial void AutomatischeAuswahl_CanExecute(ref bool result)
		{
			NextAction_CanExecute(ref result);
		}
		partial void AutomatischeAuswahl_Execute()
		{
			NextAction();
		}

		partial void NextAction_CanExecute(ref bool result)
		{
			if (this.InBearbeitung.SelectedItem == null)
			{
				result = false;
				return;
			}
			switch (InBearbeitung.SelectedItem.Status)
			{
				case (int)Bestellstatus.Bearbeitet:
				case (int)Bestellstatus.InRechnung:
				case (int)Bestellstatus.Geliefert:
				case (int)Bestellstatus.Zahlungsverzug:
					result = true;
					break;

				case (int)Bestellstatus.Bezahlt:
					result = false;
					break;

				default:
					result = !this.InBearbeitung.SelectedItem.RequiresProcessing;
					break;
			}
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
					InRechnungStellen();
					break;

				case (int)Bestellstatus.InRechnung:

					break;

				case (int)Bestellstatus.Geliefert:

					break;
			}
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

			modal = new ModalWrapper(this, FRM_NEW, TXT_NEW, "Neue Bestellung eingeben...")
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
					this.Details.Commands.Save.ExecuteAsync();
				}
			};

			InBearbeitung.SelectedItem = current;
			modal.Show();
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
			modal.Close();
		}

		partial void ArtikellisteCollection1AddAndEditNew1_Execute()
		{
			var tmp = ArtikellisteCollection.AddNew();
			if (current.Kunde != null)
				tmp.Rabatt = current.Kunde.Rabatt;

			this.ArtikellisteCollection.SelectedItem = tmp;
			ArtikellisteCollection.EditSelected();
		}

		#endregion

		#region Editieren

		partial void InBearbeitungEditSelected_CanExecute(ref bool result)
		{
			result = InBearbeitung.SelectedItem != null;
		}
		partial void InBearbeitungEditSelected_Execute()
		{
			Application.ShowBestellungDetails(InBearbeitung.SelectedItem.Id);
		}

		partial void Editieren_CanExecute(ref bool result)
		{
			result = InBearbeitung.SelectedItem != null;
		}
		partial void Editieren_Execute()
		{
			Application.ShowBestellungDetails(InBearbeitung.SelectedItem.Id);
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
			if (result == System.Windows.MessageBoxResult.Yes)
			{
				this.InBearbeitung.DeleteSelected();
				this.Details.Commands.Save.ExecuteAsync();
			}
		}
		#endregion

		#region  Versand

		partial void Versand_CanExecute(ref bool result)
		{
			var sel = InBearbeitung.SelectedItem;
			result = sel == null ? false : (sel.Status == (int)Bestellstatus.Bearbeitet) && !sel.RequiresProcessing;
		}
		partial void Versand_Execute()
		{
			StartVersendeDialog();
		}

		partial void OK_Artikel_CanExecute(ref bool result)
		{
			result = !String.IsNullOrWhiteSpace(current.Lieferscheinnummer);
		}
		partial void OK_Artikel_Execute()
		{
			modal.Close();
		}

		private void StartVersendeDialog()
		{
			current = InBearbeitung.SelectedItem;
			current.Versanddatum = DateTime.Today;
			LieferscheinDruck = false;

			modal = new ModalWrapper(this, FRM_VERSENDE, TXT_VERSENDE, "Bitte ergänzen Sie den Versanddaten...")
			{
				CancelMethod = () =>
				{
					foreach (Rechnungen item in DataWorkspace.ApplicationData.Details.GetChanges().ModifiedEntities.OfType<Rechnungen>())
						if (item.Id == current.Id)
							item.Details.DiscardChanges();
					current = null;
				},
				ProceedMethod = () =>
				{
					current.Status = (int)Bestellstatus.Versendet;
					current.RequiresProcessing = LieferscheinDruck.GetValueOrDefault(false);
					this.Details.Commands.Save.ExecuteAsync();
				}
			};
			modal.Show();
		}

		#endregion

		#region InRechnungStellen

		partial void InRechnungStellen_CanExecute(ref bool result)
		{
			var sel = InBearbeitung.SelectedItem;
			result = sel == null ? false : (sel.Status == (int)Bestellstatus.Versendet) && !sel.RequiresProcessing;
		}
		partial void InRechnungStellen_Execute()
		{
			InBearbeitung.SelectedItem.Status = (int)Bestellstatus.InRechnung;
			InBearbeitung.SelectedItem.GetRechnungsNummer();
			InBearbeitung.SelectedItem.Rechnungsdatum = DateTime.Today;
			InBearbeitung.SelectedItem.RequiresProcessing = true;
			this.Details.Commands.Save.ExecuteAsync();
		}

		#endregion

		#region Geliefert

		partial void Lieferung_CanExecute(ref bool result)
		{
			var sel = InBearbeitung.SelectedItem;
			result = sel == null ? false : (sel.Status == (int)Bestellstatus.InRechnung) && !sel.RequiresProcessing;
		}
		partial void Lieferung_Execute()
		{
			InBearbeitung.SelectedItem.Status = (int)Bestellstatus.Geliefert;
			this.Details.Commands.Save.ExecuteAsync();
		}

		#endregion

		#region Bezahlung
		partial void Bezahlt_CanExecute(ref bool result)
		{
			var sel = InBearbeitung.SelectedItem;
			result = sel == null ? false : (sel.Status == (int)Bestellstatus.Geliefert) || (sel.Status == (int)Bestellstatus.Zahlungsverzug) && !sel.RequiresProcessing;
		}
		partial void Bezahlt_Execute()
		{
			InBearbeitung.SelectedItem.Status = (int)Bestellstatus.Bezahlt;
			this.Details.Commands.Save.ExecuteAsync();
		}

		partial void Bezahlung_CanExecute(ref bool result)
		{
			Bezahlt_CanExecute(ref result);
		}
		partial void Bezahlung_Execute()
		{
			Bezahlt();
		}

		#endregion
	}
}
