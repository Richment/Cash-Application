using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
	public partial class Rechnungen
	{

		//partial void Rechnungsbetrag_Compute(ref decimal result)
	   // {
		//    result = GetSubTotal() * (decimal)0.095;
		//}

	   // protected decimal GetPreis()
	   // {
	  //      return this.ArtikellisteCollection.Sum(i => i.Preis);
	  //  }

	/*	partial void Netto_Gesamtbetrag_Compute(decimal? result)
		{
			result = this.ArtikellisteCollection.Sum(i => i.Preis);	
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}			  */

		partial void Rabattwert_Compute(ref decimal? result)
		{
			result = this.Netto_Gesamtbetrag - Rabatt;
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void Rechnungsbetrag_Netto_Compute(ref decimal? result)
		{
			result = this.Netto_Gesamtbetrag + Lieferkosten;
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void Rechnungsbetrag_Brutto_Compute(ref decimal? result)
		{
			if (this.Rechnungsbetrag_Netto.HasValue)
			{
				result = this.Rechnungsbetrag_Netto.Value + (this.Rechnungsbetrag_Netto * 19 / 100);
				if (result.HasValue)
					result = Math.Round(result.Value, 2);
			}
		}

		partial void Mehrwertsteuer_Compute(ref decimal? result)
		{
			result = this.Rechnungsbetrag_Netto * 19 / 100;
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void Adresse_Compute(ref string result)
		{
			string[] elements = new string[]
			{
				//Kunde.Firma,
				Kunde.Vorname +' '+ Kunde.Nachnahme,
				Kunde.Straße +' '+ Kunde.Hausnummer,
				Kunde.PLZ +' '+ Kunde.Stadt,
				Kunde.Land
			};
			result = String.Join(Environment.NewLine, elements);
		}
	}
}
