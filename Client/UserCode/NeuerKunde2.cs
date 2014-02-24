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
    public partial class NeuerKunde2
    {
        partial void NeuerKunde2_InitializeDataWorkspace(global::System.Collections.Generic.List<global::Microsoft.LightSwitch.IDataService> saveChangesTo)
        {
            // Erstellen Sie hier Ihren Code.
            this.KundenProperty = new Kunden();
        }

        partial void NeuerKunde2_Saved()
        {
            // Erstellen Sie hier Ihren Code.
            this.Close(false);
            Application.Current.ShowDefaultScreen(this.KundenProperty);
        }
    }
}