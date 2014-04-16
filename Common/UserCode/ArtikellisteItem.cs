
namespace LightSwitchApplication
{
	using System;
	using System.Linq;

	public partial class ArtikellisteItem
	{
		partial void ArtikellisteItem_Created()
		{
			this.Anzahl = 1;
		}

		partial void PosPreis_Compute(ref decimal result)
		{
			if (ArtikelstammItem != null)
				result = Math.Round(ArtikelstammItem.VK_pro_PK, 2);
		}

		partial void Position_Compute(ref int result)
		{
			if (this.Rechnungen != null)
				result = this.Rechnungen.ArtikellisteCollection.ToList().IndexOf(this) + 1;
		}

		partial void Artikelnummer_Compute(ref string result)
		{
			if (ArtikelstammItem != null)
				result = ArtikelstammItem.Artikelnummer.ToString();
		}

		partial void Bezeichnung_Compute(ref string result)
		{
			if (ArtikelstammItem != null)
				result = ArtikelstammItem.Artikelbeschreibung;
		}

		partial void Preis_Compute(ref decimal result)
		{
			if (ArtikelstammItem != null)
				result = Math.Round(Anzahl * ArtikelstammItem.VK_pro_PK, 2) - Rabattwert;
		}

		partial void Rabattwert_Compute(ref decimal result)
		{
			if (ArtikelstammItem != null)
				result = Math.Round(Anzahl * PosPreis * (Rabatt / 100M), 2);
		}

		partial void AnzeigeName_Compute(ref string result)
		{
			if (ArtikelstammItem != null)
				result = ArtikelstammItem.ToString();
		}

		public override string ToString()
		{
			if (ArtikelstammItem != null)
				return ArtikelstammItem.Artikelbeschreibung;
			return base.ToString();
		}
	};
}
