namespace LightSwitchApplication
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	public static class KundenItemExtension
	{
		public static string ToDIN5008B(this KundenItem value)
		{
			List<string> result = new List<string>();
			if (!String.IsNullOrWhiteSpace(value.Firma))
				result.Add(value.Firma);
			else if (!String.IsNullOrWhiteSpace(value.Anrede))
				result.Add(value.Anrede);

			var line = String.Empty;
			if (!String.IsNullOrWhiteSpace(value.Vorname))
				line = value.Vorname + " ";
			if (!String.IsNullOrWhiteSpace(value.Nachnahme))
				line = value.Nachnahme;
			result.Add(line.Trim());

			if (!String.IsNullOrWhiteSpace(value.Straße))
			{
				line = value.Straße;
				if (value.Hausnummer.HasValue)
					line += " " + value.Hausnummer.Value.ToString();
				result.Add(line);
			}

			if (value.PLZ.HasValue)
				line = value.PLZ.Value.ToString();
			if (!String.IsNullOrWhiteSpace(value.Stadt))
				line += " " + value.Stadt;
			if (!String.IsNullOrWhiteSpace(line))
				result.Add(line.Trim());
			if (!String.IsNullOrWhiteSpace(value.Land))
				result.Add(value.Land);

			return String.Join(Environment.NewLine, result.ToArray());
		}
	};
}
