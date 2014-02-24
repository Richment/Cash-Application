using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
    public partial class Artikelliste
    {

        partial void Preis_Compute(ref decimal? result)
        {
			result = this.Anzahl * Artikelstamm.VK_pro_PK;
        }

        partial void PosPreis_Compute(ref decimal result)
        {
			result = 1 * Artikelstamm.VK_pro_PK;
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
