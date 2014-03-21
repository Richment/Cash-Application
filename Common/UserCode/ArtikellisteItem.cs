using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class ArtikellisteItem
	{
		partial void ArtikellisteItem_Created()
		{
			this.Anzahl = 1;
		}

		partial void PosPreis_Compute(ref decimal result)
		{
			if (ArtikelstammItem != null)
				result = Math.Round(this.ArtikelstammItem.VK_pro_PK, 2);
		}

		partial void Position_Compute(ref int result)
		{		 
			if(Rechnungen!=null)
				result = this.Rechnungen.ArtikellisteCollection.ToList().IndexOf(this) + 1;
		}

		partial void Artikelnummer_Compute(ref string result)
		{	
			if (ArtikelstammItem != null)
				result = this.ArtikelstammItem.Artikelnummer.ToString();
		}

		partial void Bezeichnung_Compute(ref string result)
		{ 
			if (ArtikelstammItem != null)
				result = this.ArtikelstammItem.Artikelbeschreibung;
		}

		partial void Preis_Compute(ref decimal result)
		{
			if (ArtikelstammItem != null)
				result = Math.Round(this.Anzahl * this.ArtikelstammItem.VK_pro_PK, 2) - this.Rabattwert;
		}

		partial void Rabattwert_Compute(ref decimal result)
		{
			if (ArtikelstammItem != null)
				result = Math.Round(this.Anzahl * this.PosPreis * (this.Rabatt / 100M), 2);
		}

		public override string ToString()
		{
			if (ArtikelstammItem != null)
				return this.ArtikelstammItem.Artikelbeschreibung;
			return base.ToString();
		}

		partial void AnzeigeName_Compute(ref string result)
		{
			if (ArtikelstammItem != null)
				result = ArtikelstammItem.ToString();
		}
	}
}
