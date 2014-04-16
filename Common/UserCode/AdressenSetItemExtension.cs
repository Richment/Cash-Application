using System;
using System.Collections.Generic;

namespace LightSwitchApplication
{
	public static class AdressenSetItemExtension
	{
		public static string ToDIN5008B(this AdressenSetItem value)
		{
			List<string> result = new List<string>();

			if (!String.IsNullOrWhiteSpace(value.Anrede))
				result.Add(value.Anrede);
			if (!String.IsNullOrWhiteSpace(value.Name))
				result.Add(value.Name);
			if (!String.IsNullOrWhiteSpace(value.zHd_Besteller_optional))
				result.Add(value.zHd_Besteller_optional);
			var line = String.Empty;
			if (!String.IsNullOrWhiteSpace(value.Straße))
			{
				line = value.Straße;
				if (!String.IsNullOrWhiteSpace(value.Hausnummer))
					line += " " + value.Hausnummer;
				result.Add(line);
			}
			line = String.Empty;
			if (!String.IsNullOrWhiteSpace(value.PLZ))
				line = value.PLZ;
			if (!String.IsNullOrWhiteSpace(value.Stadt))
				line += " " + value.Stadt;
			if (!String.IsNullOrWhiteSpace(line))
				result.Add(line);
			if (!String.IsNullOrWhiteSpace(value.Land))
				result.Add(value.Land);

			return String.Join(Environment.NewLine, result.ToArray());
		}
	};
}
