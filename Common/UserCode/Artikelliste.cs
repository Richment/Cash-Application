using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class Artikelliste
	{

		partial void Preis_Compute(ref decimal result)
		{
			result = Math.Round(this.Anzahl * Artikelstamm.VK_pro_PK, 2);
		}

		partial void PosPreis_Compute(ref decimal result)
		{
			result = Math.Round(Artikelstamm.VK_pro_PK, 2);
		}

		partial void Position_Compute(ref int result)
		{
			result = this.Position + 1;
		}

		partial void Artikelnummer_Compute(ref string result)
		{
			result = Artikelstamm.Artikelnummer.ToString();
		}

		partial void Bezeichnung_Compute(ref string result)
		{
			result = Artikelstamm.Artikelbeschreibung;
		}
	}
}
