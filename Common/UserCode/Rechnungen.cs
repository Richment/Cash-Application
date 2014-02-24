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

        partial void Netto_Gesamtbetrag_Compute(ref decimal? result)
        {
            result = this.ArtikellisteCollection.Sum(i => i.Preis);
        }

        partial void Rabattwert_Compute(ref decimal? result)
        {
            result = this.Netto_Gesamtbetrag - Rabatt;
        }

        partial void Rechnungsbetrag_Netto_Compute(ref decimal? result)
        {
            result = this.Netto_Gesamtbetrag + Lieferkosten;
        }

        partial void Rechnungsbetrag_Brutto_Compute(ref decimal? result)
        {
            result = this.Rechnungsbetrag_Netto + (this.Rechnungsbetrag_Netto*19/100);
        }

        partial void Mehrwertsteuer_Compute(ref decimal? result)
        {
            result = this.Rechnungsbetrag_Netto * 19 / 100;
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
