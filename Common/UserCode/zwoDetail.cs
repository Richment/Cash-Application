using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;

namespace LightSwitchApplication
{
    public partial class zwoDetail
    {
        partial void zwoDetail_InitializeDataWorkspace(global::System.Collections.Generic.List<global::Microsoft.LightSwitch.IDataService> saveChangesTo)
        {
            // Erstellen Sie hier Ihren Code.
            this.RechnungenProperty = new Rechnungen();
        }

        partial void zwoDetail_Saved()
        {
            // Erstellen Sie hier Ihren Code.
            this.Close(false);
            Application.Current.ShowDefaultScreen(this.RechnungenProperty);
        }

        partial void Netto_Gesamtbetrag_Compute(ref decimal? result)
        {
            result = this.ArtikellisteCollection.Sum(i => i.Preis);

        }

    }
}