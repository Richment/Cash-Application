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
    public partial class ListDetail_Rechnungen
    {
    /*    partial void Drucken_Execute()
        {


            //Export all data to the bookmarked "PlainTable" 
            //Word.Export(this.Application.WordExportDocument, "PlainTable", 1, true, this.RechnungenSet);

            //'Export only particular fields to the bookmarked "MapTable"
            var mappings = new List<ColumnMapping> { };
            mappings.Add(new ColumnMapping("Id", "Position"));
            mappings.Add(new ColumnMapping("Artikelbezeichnung", "Artikelbezeichnung"));
            mappings.Add(new ColumnMapping("", "Author"));

            Word.Export(this.Application.WordExportDocument, "MapTable", 1, true, this.RechnungenSet, mappings);

            //'Export only particular fields to the bookmarked "MapTableNoHeading",
            //' don't generate column headings, and start at row 2 of the table
            Word.Export(this.Application.WordExportDocument, "MapTableNoHeading", 2, false, this.RechnungenSet, mappings);
           
           
         }
        

          
        partial void Drucken_CanExecute(ref bool result)
        {
           
        }
    
     */
    }
}
