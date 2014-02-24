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
    public partial class DruckenRechnungenDetail
    {
        partial void Rechnungen_Loaded(bool succeeded)
        {
            // Erstellen Sie hier Ihren Code.
            this.SetDisplayNameFromEntity(this.Rechnungen);
        }

        partial void Rechnungen_Changed()
        {
            // Erstellen Sie hier Ihren Code.
            this.SetDisplayNameFromEntity(this.Rechnungen);
        }

        partial void DruckenRechnungenDetail_Saved()
        {
            // Erstellen Sie hier Ihren Code.
            this.SetDisplayNameFromEntity(this.Rechnungen);
        }

        partial void Drucken_Execute()
        {
            //'Export only particular fields to the bookmarked "MapTable"
            var mappings = new List<ColumnMapping> { };
            mappings.Add(new ColumnMapping("", "Rechnungsnummer"));
            mappings.Add(new ColumnMapping("", "Gesamtbetrag"));
            mappings.Add(new ColumnMapping("", "Anzahl"));

           // Word.Export(this.Application.WordExportDocument, "MapTable", 1, true, this.RechnungenSet, mappings);

            //Export a specific set of Book fields to a new workbook
            var fields = new List<String> { "Title", "Author" };


          //  OfficeIntegration.Word.Export(this.RechnungenSet, true, fields, false);
        }
    }
}