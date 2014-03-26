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
	public partial class AuftragsSammlung
	{
		partial void ProcessList_CanExecute(ref bool result)
		{
			result = this.AuftragsSammlung1.Count > 0;
		}

		partial void ProcessList_Execute()
		{
			Dictionary<Anbieter, List<string>> collection = new Dictionary<Anbieter,List<string>>();
			
			var bestellungen = this.AuftragsSammlung1.Where(n => n.Status == (int)Bestellstatus.Neu);
			
			foreach (var item in bestellungen)
			{
				var grouped = item.ArtikellisteCollection.GroupBy(n => n.ArtikelstammItem.Anbieter, n => n);
				foreach (IGrouping<Anbieter, ArtikellisteItem> groupedItem in grouped)
				{
					var key =  groupedItem.Key ;
					if (!collection.ContainsKey(key))
						collection.Add(key, new List<string>());

					collection[key].Add(String.Format("Referenznummer: {0}", item.Referenznummer));
					collection[key].Add("Lieferadresse:");
					
					if (item.Lieferadresse == null)
					{
						if (!String.IsNullOrWhiteSpace(item.Kunde.Firma))
							collection[key].Add("   " + item.Kunde.Firma);
						collection[key].Add("   "+(item.Kunde.Vorname + ' ' + item.Kunde.Nachnahme).Trim());
						collection[key].Add("   " + item.Kunde.Straße + ' ' + item.Kunde.Hausnummer);
						collection[key].Add("   " + item.Kunde.PLZ + ' ' + item.Kunde.Stadt);
						collection[key].Add("   " + item.Kunde.Land);
					}
					else
					{
						if (!String.IsNullOrWhiteSpace(item.Lieferadresse.zHd_Besteller_optional))
							collection[key].Add("   " + item.Lieferadresse.zHd_Besteller_optional);
						collection[key].Add("   " + item.Lieferadresse.Name);
						collection[key].Add("   " + item.Lieferadresse.Starße + ' ' + item.Lieferadresse.Hausnummer);
						collection[key].Add("   " + item.Lieferadresse.PLZ + ' ' + item.Lieferadresse.Stadt);
						collection[key].Add("   " + item.Lieferadresse.Land);
					}
					
					collection[key].Add("");
					collection[key].Add("  Anzahl     Artikelnummer");
					foreach (ArtikellisteItem artikel in groupedItem)
					{
						collection[key].Add(String.Format("  {0}   x   {1}", artikel.Anzahl.ToString().PadLeft(4, ' '), artikel.Artikelnummer));
					}
					collection[key].Add("");
					collection[key].Add("");	
					collection[key].Add("");
				}

				//item.RequiresProcessing = false;
				//item.Status = (int)Bestellstatus.Bearbeitet;
			}
	
			foreach (var item in collection)
			{
				this.ShowMessageBox(String.Join(Environment.NewLine, item.Value.ToArray()));				
				/*var newItem = OutgoingMailSet.AddNew();
				newItem.Recipient = item.Key.Email;
				newItem.Subject = "Neue Bestellungen vom " + DateTime.Today.ToShortDateString();
				newItem.Body = String.Join(Environment.NewLine, item.Value.ToArray());*/
			}

			this.Save();
			this.Refresh();
		}
	}
}
