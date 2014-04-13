namespace LightSwitchApplication
{
	using System;
	using System.Linq;

	public static class RechnungenExtension
	{
		public static string GetAuftragsNummer(this Rechnungen value)
		{
			var today = DateTime.Today;
			var dateString = (today.Year - 2000).ToString().PadLeft(2, '0') + today.Month.ToString().PadLeft(2, '0') + today.Day.ToString().PadLeft(2, '0');
			string result = dateString + "000";

			using (var dw = Application.Current.CreateDataWorkspace())
			{
				var query = dw.ApplicationData.RechnungenSet.OfType<Rechnungen>();
				if (query.Count() > 0)
				{
					string max = query.Max(n => n.Referenznummer);

					if (max.StartsWith(dateString))
					{
						long numeric;
						if (Int64.TryParse(max, out numeric))
						{
							result = (++numeric).ToString().PadLeft(9, '0');
						}
					}
				}
			}
			return result;
		}
		public static void GetRechnungsNummer(this Rechnungen value)
		{
			var today = DateTime.Today;
			var dateString = today.Year.ToString() + '-';
			string result = dateString + "001";

			using (var dw = Application.Current.CreateDataWorkspace())
			{
				var query = dw.ApplicationData.RechnungenSet.OfType<Rechnungen>();
				if (query.Count() > 0)
				{
					string max = query.Where(n => !String.IsNullOrWhiteSpace(n.Rechnungsnummer)).Max(n => n.Rechnungsnummer);

					if (max != null)
					{
						if (max.StartsWith(dateString))
						{
							int numeric;
							if (Int32.TryParse(max.Substring(5), out numeric))
							{
								result = dateString + (++numeric).ToString().PadLeft(3, '0');
							}
						}
					}
				}
			}
			value.Rechnungsnummer = result;
		}
	};
}
