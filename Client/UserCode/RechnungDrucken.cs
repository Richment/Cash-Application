using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.LightSwitch.Presentation.Extensions;
using OfficeIntegration;

namespace LightSwitchApplication
{
	public partial class RechnungDrucken
	{
		private string currentFilename;

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

		partial void RechnungDrucken_Saved()
		{
			// Erstellen Sie hier Ihren Code.
			this.SetDisplayNameFromEntity(this.Rechnungen);
		}

		partial void Drucken_Execute()
		{
			if ((currentFilename = CreateTemplate()) == null)
				return;

			using (WordDocument doc = new WordDocument(currentFilename, ProcessDocument))
			{
				try
				{
					/*var artikelFields = new List<string>() 
					{
						"Position", 
						"Artikelnummer", 
						"Anzahl",
						"PosPreis", 
						"Preis" 
					};

					object wdoc = Word.GetDocument(Word.GetWord(), doc.Filename);
					Word.Export(wdoc, "MapTableNoHeading", 2, false, this.ArtikellisteCollection, artikelFields);	  */

					if (!doc.Print())
						this.ShowError(new Exception("Dokument konnte nicht gedruckt werden."));
				}
				catch (Exception ex)
				{
					this.ShowError(ex);
				}
			}
		}


		partial void SaveDocument_Execute()
		{
			string filename = this.FilenameInputBox("Bitte geben Sie einen Dateinamen an.", "Datei speichern...", "docx", "Neue Rechnung");

			if ((currentFilename = CreateTemplate()) == null)
				return;

			/*    List<ColumnMapping> mappings = new List<ColumnMapping>()
				{
					 new ColumnMapping("Adresse","Adresse"),
				};
			
				List<string> artikelFields = new List<string>() 
				{
					"Position", 
					"ArtikelStamm.ArtikelNummer", 
					"Anzahl",
					"PosPreis", 
					"Preis" 
				};
			
				object wdoc = Word.GenerateDocument(tempFilename, this.ArtikellisteCollection.SelectedItem.Rechnungen, mappings);
			
				Word.Export(wdoc, "MapTableNoHeading", 2, false, this.ArtikellisteCollection, artikelFields);

				Word.SaveAsPDF(wdoc, @"C:\users\richment\desktop\xx__XX__xx.pdf",false);
			
				return;			 */

			using (WordDocument doc = new WordDocument(currentFilename, ProcessDocument))
			{
				try
				{
					if (!doc.SaveAs(filename))
					{
						this.ShowMessage("Dokument konnte nicht gespeichert werden.");
						return;
					}

					doc.Close(true);
				}
				catch (Exception ex)
				{
					this.ShowError(ex);
				}

				if (MessageBoxResult.Yes == this.ShowMessageBox("Möchten Sie das Dokument jetzt öffnen?", "Dokument erfolgreich gespeichert.", MessageBoxOption.YesNo))
				{
					try
					{
						Helper.ShellExecute(filename);
					}
					catch (Exception ex)
					{
						this.ShowError(ex);
					}
				}
			}
		}


		private void ProcessDocument(dynamic wordApp)
		{
			var wordDoc = wordApp.ActiveDocument;

			//wordDoc.Bookmarks("Firma").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Kunde.Firma);
			wordDoc.Bookmarks("Adresse").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Adresse);
			wordDoc.Bookmarks("rechnungsnr").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Rechnungsnummer);
			wordDoc.Bookmarks("rechnungsdatum").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Rechnungsdatum);
			wordDoc.Bookmarks("auftragsnr").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Auftragsnummer);
			wordDoc.Bookmarks("lieferscheinnr").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Lieferscheinnummer);
			wordDoc.Bookmarks("lieferdatum").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Lieferdatum);
			wordDoc.Bookmarks("lieferkosten").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Lieferkosten);
			wordDoc.Bookmarks("gesamtnetto").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Rechnungsbetrag_Netto);
			wordDoc.Bookmarks("mehrwertsteuer").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Mehrwertsteuer);
			wordDoc.Bookmarks("brutto").Range.InsertAfter(ArtikellisteCollection.SelectedItem.Rechnungen.Rechnungsbetrag_Brutto);

			var artikelFields = new List<string>() 
			{
				"Position", 
				"Artikelnummer", 
				"Bezeichnung",
				"Anzahl",
				"PosPreis", 
				"Preis" 
			};

			object wdoc = Word.GetDocument(Word.GetWord(), currentFilename);
			Word.Export(wdoc, "MapTableNoHeading", 2, false, this.ArtikellisteCollection, artikelFields);

			// wordApp.Visible = true;
		}

		private string CreateTemplate()
		{
			var tempFilename = Path.Combine(Path.GetTempPath(), DateTime.UtcNow.Ticks.ToString() + ".docx");
			var resName = Application.Current.Details.Name + ".Template.docx";
			using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName))
			{
				if (resourceStream == null)
					return null;

				using (FileStream fs = new FileStream(tempFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					resourceStream.CopyTo(fs);
					fs.Flush(true);
				}
			}
			return tempFilename;
		}
	}
}

/* string WordFile = MyDocs + "\\RechnungExport.docx";


 if (File.Exists(WordFile))
 {
	 //Map the content control tag names in the word document to the entity field names
	// List<ColumnMapping> custFields = new List<ColumnMapping>();

	// custFields.Add(new ColumnMapping("PLZ", "PLZ"));

	 // neues doc öffnen...    dynamic doc = OfficeIntegration.Word.GenerateDocument(WordFile, this.Rechnungen.Kunde, custFields);
			   

	// List<ColumnMapping> orderFields = new List<ColumnMapping>();
	// orderFields.Add(new ColumnMapping("Rechnungsnummer", "Rechnungsnummer"));


	 //........................Tabelle.......................


	 //Export all data to the bookmarked "PlainTable" 
	 //Word.Export(this.Application.WordExportDocument, "PlainTable", 1, true, this.ArtikellisteCollection);

	 //'Export only particular fields to the bookmarked "MapTable"
	 var mappings = new List<ColumnMapping> { };
	// mappings.Add(new ColumnMapping("", "Position"));
	 mappings.Add(new ColumnMapping("", "Artikelnummer"));
	 mappings.Add(new ColumnMapping("", "Artikelstamm"));
	 mappings.Add(new ColumnMapping("", "Anzahl"));
	 mappings.Add(new ColumnMapping("", "Preis"));
	 //mappings.Add(new ColumnMapping("", "PosPreis"));
			

	 // Word.Export(this.Application.WordExportDocument, "MapTable", 1, true, this.ArtikellisteCollection, mappings);

	 //'Export only particular fields to the bookmarked "MapTableNoHeading",
	 //' don't generate column headings, and start at row 2 of the table
	 Word.Export(this.Application.WordExportDocument, "MapTableNoHeading", 2, false, this.ArtikellisteCollection, mappings);


			
 }

 */

