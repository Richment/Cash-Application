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
    public partial class CreateNewAdressenSetItem
    {
        partial void CreateNewAdressenSetItem_InitializeDataWorkspace(global::System.Collections.Generic.List<global::Microsoft.LightSwitch.IDataService> saveChangesTo)
        {
            // Erstellen Sie hier Ihren Code.
            this.AdressenSetItemProperty = new AdressenSetItem();
        }

        partial void CreateNewAdressenSetItem_Saved()
        {
            // Erstellen Sie hier Ihren Code.
            this.Close(false);
            Application.Current.ShowDefaultScreen(this.AdressenSetItemProperty);
        }
    }
}