namespace LightSwitchApplication
{
	using System;
	using System.Linq;
	using Microsoft.LightSwitch;

	public partial class Application
	{
		public string StartScreenMessage
		{
			get;
			private set;
		}

		partial void Application_Initialize()
		{
			var today = DateTime.Today;
			
			// Prüfe Dokumentenvorlage
			using (var dw = Application.Current.CreateDataWorkspace())
			{
				var defaultItem = dw.ApplicationData.ReportingTemplatesSet.FirstOrDefault();
				if (defaultItem == null)
				{
					defaultItem = dw.ApplicationData.ReportingTemplatesSet.AddNew();
					defaultItem.ReleaseDate = ReportingTemplates.Minimum;
					defaultItem.OriginalFilename = " ";			
					defaultItem.Template = new byte[] { };

					dw.ApplicationData.SaveChanges();
				}
			}

			// Setze Zahlungsverzug
			using (var dw = Application.Current.CreateDataWorkspace())
			{
				foreach (var item in dw.ApplicationData.Prüfaufgaben().OfType<Rechnungen>())
					if ((item.Rechnungsdatum ?? DateTime.MinValue).AddDays(item.Kunde.Zahlungsziel) > today)
					{
						item.Status = (int)Bestellstatus.Zahlungsverzug;
						item.RequiresProcessing = true;
					}

				dw.ApplicationData.SaveChanges();
			}
			
			// Zähle Zahlungsverzug
			using (var dw = Application.Current.CreateDataWorkspace())
			{
				var query = dw.ApplicationData.Zahlungsverzug().OfType<Rechnungen>();
				int invoices = query.Count();
				int customers = query.GroupBy(n => n.Kunde).Count();
				StartScreenMessage = invoices == 0 ? String.Empty : String.Format("{0} Kunden sind mit {1} Rechnungen in Verzug.", customers, invoices);
			}
		}
	};
}
