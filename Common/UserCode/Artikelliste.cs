using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class Artikelliste
	{
		partial void PosPreis_Compute(ref decimal result)
		{
			if (Artikelstamm != null)
				result = Math.Round(this.Artikelstamm.VK_pro_PK, 2);
		}

		partial void Position_Compute(ref int result)
		{
			result = this.Position + 1;
		}

		partial void Artikelnummer_Compute(ref string result)
		{	
			if (Artikelstamm != null)
				result = this.Artikelstamm.Artikelnummer.ToString();
		}

		partial void Bezeichnung_Compute(ref string result)
		{ 
			if (Artikelstamm != null)
				result = this.Artikelstamm.Artikelbeschreibung;
		}

		partial void Preis_Compute(ref decimal result)
		{
			if (Artikelstamm != null)
				result = Math.Round(this.Anzahl * this.Artikelstamm.VK_pro_PK, 2) - this.Rabattwert;
		}

		partial void Rabattwert_Compute(ref decimal result)
		{
			if (Artikelstamm != null)
				result = Math.Round(this.Anzahl * this.PosPreis * (this.Rabatt / 100M), 2);
		}
	}
}
