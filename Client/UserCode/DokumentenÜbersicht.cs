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
			if (value.GeneratedData != null)
			{
				File.WriteAllBytes(path, value.GeneratedData);
				Helper.ShellExecute(path);
			}
		}

		partial void DocumentsListEditSelected_CanExecute(ref bool result)
		{
			result = DocumentsSet.SelectedItem != null;
		}

		partial void DocumentsListEditSelected_Execute()
		{
			var value = DocumentsSet.SelectedItem;
			value.Datum = DateTime.Now;
			value.GeneratedData = null;
			Save();
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
				File.WriteAllBytes(filename, value.GeneratedData);
			}
		}

	}
}
