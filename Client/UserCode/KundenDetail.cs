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
    public partial class KundenDetail
    {
        partial void Kunden_Loaded(bool succeeded)
        {
            // Erstellen Sie hier Ihren Code.
            this.SetDisplayNameFromEntity(this.Kunden);
        }

        partial void Kunden_Changed()
        {
            // Erstellen Sie hier Ihren Code.
            this.SetDisplayNameFromEntity(this.Kunden);
        }

        partial void KundenDetail_Saved()
        {
            // Erstellen Sie hier Ihren Code.
            this.SetDisplayNameFromEntity(this.Kunden);
        }
    }
}