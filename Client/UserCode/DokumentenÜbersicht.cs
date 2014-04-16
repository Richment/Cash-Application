using System;
using System.IO;

namespace LightSwitchApplication
{
	public partial class DokumentenÜbersicht
	{
		partial void ShowDocument_CanExecute(ref bool result)
		{
			result = DocumentsSet.SelectedItem != null;
		}
		partial void ShowDocument_Execute()
		{
			var value = DocumentsSet.SelectedItem;
			string path = Helper.GetFreeTempFilename("pdf");
			if (value.GeneratedDocument != null)
			{
				File.WriteAllBytes(path, value.GeneratedDocument.Bytes);
				Helper.ShellExecute(path);
			}
		}

		partial void SaveDocument_CanExecute(ref bool result)
		{
			result = DocumentsSet.SelectedItem != null;
		}
		partial void SaveDocument_Execute()
		{
			var value = DocumentsSet.SelectedItem;
			string filename = null;
			if (!String.IsNullOrWhiteSpace(filename = this.FilenameInputBox("Bitte geben Sie einen Dateinamen für dieses Dokument an.", "Dokument speichern...", ".pdf", value.Bezeichnung)))
			{
				File.WriteAllBytes(filename, value.GeneratedDocument.Bytes);
				this.ShowMessage("Das Dokument wurde unter{0}{1}{0}gespeichert.", Environment.NewLine, Path.GetFullPath(filename));
			}
		}

		partial void PrintDocument_CanExecute(ref bool result)
		{
			result = DocumentsSet.SelectedItem != null;
		}
		partial void PrintDocument_Execute()
		{
			var value = DocumentsSet.SelectedItem;
			string path = Helper.GetFreeTempFilename("pdf");
			if (value.GeneratedDocument != null)
			{
				File.WriteAllBytes(path, value.GeneratedDocument.Bytes);
				Helper.ShellExecute(path, operation: ProcessVerb.Print);
			}
		}
	}
}
