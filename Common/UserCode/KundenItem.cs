namespace LightSwitchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.LightSwitch;

	public partial class KundenItem
	{
		partial void Kundennummer_Compute(ref string result)
		{
			result = "KA" + Id.ToString().PadLeft(6, '0');
		}

		public override string ToString()
		{
			if ((Kundennummer == null) && (Firma == null) && (Vorname == null) && (Nachnahme == null))
				return base.ToString();

			string result = String.IsNullOrWhiteSpace(Kundennummer) ? "" : Kundennummer + " - ";
			result += String.IsNullOrWhiteSpace(this.Firma) ? "" : this.Firma + ", ";
			result += ((this.Anrede ?? "").Trim() + ' ' + this.Nachnahme.Trim() + ' ' + this.Vorname.Trim()).Trim();
			return String.IsNullOrWhiteSpace(result) ? base.ToString() : result;
		}
	};
}
