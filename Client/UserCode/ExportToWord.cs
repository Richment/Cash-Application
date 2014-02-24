using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
using OfficeIntegration;
namespace LightSwitchApplication
{
    public partial class ExportToWord
    {
        partial void Drucken_Execute()
        {
            //Export a specific set of Book fields to a new workbook
            var fields = new List<String> { "Rechnungsnummer", "Id" };

            OfficeIntegration.Word.Export(this.RechnungenSet, false, fields, false);

        }






        partial void Drucken_CanExecute(ref bool result)
        {
            // Erstellen Sie hier Ihren Code.

        }
    }
}
