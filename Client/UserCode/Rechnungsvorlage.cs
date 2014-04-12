using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
using Microsoft.LightSwitch.Threading;
using System.Collections.Specialized;

namespace LightSwitchApplication
{
	public partial class Rechnungsvorlage
	{
		private static string description;

		partial void Import_CanExecute(ref bool result)
		{
			result = true;
		}

		partial void Import_Execute()
		{
			description = this.ShowInputBox("Bitte geben Sie eine Beschreibung ein.", "Neue Vorlage...");

			Dispatchers.Main.BeginInvoke(() =>
			{
				SelectFileWindow selectFileWindow = new SelectFileWindow()
				{
					Text = "Bitte geben Sie die Datei an, die Sie als neue Vorlage verwenden möchten.",
					Filter = "Worddokumente (*.docx)|*.docx|Alle Dateien (*.*)|*.*"
				};
				selectFileWindow.Closed += new EventHandler(SelectFileWindow_Closed);
				selectFileWindow.Show();
			});
		}

		private void SelectFileWindow_Closed(object sender, EventArgs e)
		{
			SelectFileWindow selectFileWindow = (SelectFileWindow)sender;

			byte[] bytes = null;
			if (selectFileWindow.DialogResult.GetValueOrDefault(false) && (selectFileWindow.DocumentStream != null))
			{
				bytes = new byte[selectFileWindow.DocumentStream.Length];
				using (StreamReader streamReader = new StreamReader(selectFileWindow.DocumentStream))
				{
					for (int i = 0; i < selectFileWindow.DocumentStream.Length; i++)
						bytes[i] = (byte)selectFileWindow.DocumentStream.ReadByte();
				}

				selectFileWindow.DocumentStream.Close();
				selectFileWindow.DocumentStream.Dispose();
			}

			if (bytes == null)
				return;

			string filename = selectFileWindow.SafeFileName;
			
			Details.Dispatcher.EnsureInvoke(() =>
			{	
				using (var dw = Application.Current.CreateDataWorkspace())
				{
					var newItem = dw.ApplicationData.ReportingTemplatesSet.AddNew();
					newItem.OriginalFilename = filename;
					newItem.ReleaseDate = DateTime.Now;
					newItem.Beschreibung = description ?? "";
					newItem.Template = bytes;
					dw.ApplicationData.SaveChanges();
				}
				Refresh();
			});
		}

		partial void View_CanExecute(ref bool result)
		{
			result = ReportingTemplatesSet.SelectedItem != null;
		}

		partial void View_Execute()
		{
			var sel = ReportingTemplatesSet.SelectedItem;
			string temp = Helper.GetFreeTempFilename(Path.GetExtension(sel.OriginalFilename));
			File.WriteAllBytes(temp, sel.Template);
			Helper.ShellExecute(temp);
		}

		partial void ReportingTemplatesSet_Changed(NotifyCollectionChangedEventArgs e)
		{
			CurrentReport = ReportingTemplatesSet.OrderBy(n => n.ReleaseDate).LastOrDefault();
		}

		partial void ViewCurrent_CanExecute(ref bool result)
		{
			result = CurrentReport != null;
		}

		partial void ViewCurrent_Execute()
		{
			string temp = Helper.GetFreeTempFilename(Path.GetExtension(CurrentReport.OriginalFilename));
			File.WriteAllBytes(temp, CurrentReport.Template);
			Helper.ShellExecute(temp);
		}
	};
}
