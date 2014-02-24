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
    public partial class CreateFirmendaten
    {
        partial void CreateFirmendaten_InitializeDataWorkspace(List<IDataService> saveChangesTo)
        {
            // Erstellen Sie hier Ihren Code.
            this.FirmendatenProperty = new Firmendaten();
        }

        partial void CreateFirmendaten_Saved()
        {
            // Erstellen Sie hier Ihren Code.
            this.Close(false);
            Application.Current.ShowDefaultScreen(this.FirmendatenProperty);
        }

        partial void FirmendatenProperty_Validate(ScreenValidationResultsBuilder results)
        {
            // results.AddPropertyError("<Fehlermeldung>");

        }
    }
}