namespace LightSwitchApplication
{
	using System;
	using System.Linq;
	using System.IO;
	using System.IO.IsolatedStorage;
	using System.Collections.Generic;
	using Microsoft.LightSwitch;
	using Microsoft.LightSwitch.Framework.Client;
	using Microsoft.LightSwitch.Presentation;
	using Microsoft.LightSwitch.Presentation.Extensions;
	using Microsoft.LightSwitch.Threading;
	using System.Windows;
	using Microsoft.LightSwitch.Client;
	using Microsoft.LightSwitch.Details;
	using System.Threading;

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
