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

	public partial class Application
	{
		public static string StartScreenMessage
		{
			get;
			private set;
		}

		partial void Application_Initialize()
		{
			CheckDelayedPayment();
			SetStartScreenMessage();
		}

		private void SetStartScreenMessage()
		{
			using (var dw = Application.Current.CreateDataWorkspace())
			{
				var query = dw.ApplicationData.Zahlungsverzug().OfType<Rechnungen>();
				int invoices = query.Count();
				int customers = query.GroupBy(n => n.Kunde).Count();
				StartScreenMessage = invoices == 0 ? String.Empty : String.Format("{0} Kunden sind mit {1} Rechnungen in Verzug.", customers, invoices);
			}
		}

		internal static void CheckDelayedPayment()
		{
			var today = DateTime.Today;

			using (var dw = Application.Current.CreateDataWorkspace())
			{
				foreach (var item in dw.ApplicationData.InRechnungGestellt().OfType<Rechnungen>())
					if ((item.Lieferdatum ?? DateTime.MinValue).AddDays(item.Kunde.Zahlungsziel) > today)
						item.Status = (int)Bestellstatus.Zahlungsverzug;

				dw.ApplicationData.SaveChanges();
			}
		}
	};
}
