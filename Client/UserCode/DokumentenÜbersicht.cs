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
			string path = GetFreeFilename();
			File.WriteAllBytes(path, value.Data);
			Helper.ShellExecute(path);
		}

		private static string GetFreeFilename()
		{
		 	byte[] num = new byte[4];
			string result = null;
			do
			{
				try
				{
					result = Path.Combine(Path.GetTempPath(), BitConverter.ToUInt32(num, 0).ToString() + ".docx");
				}
				catch
				{
					continue;
				}
			} while (File.Exists(result));
			return result;
		}
	}
}
