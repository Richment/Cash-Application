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

    }
}
