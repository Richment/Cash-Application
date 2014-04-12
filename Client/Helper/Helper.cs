namespace LightSwitchApplication
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.InteropServices.Automation;
	using System.Windows.Controls;
	using Microsoft.LightSwitch.Client;
	using Microsoft.LightSwitch.Presentation;
	using Microsoft.LightSwitch.Presentation.Extensions;

	/// <summary>
	/// This static class provides multiple helper-methods and extensions.
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// Displays a <see cref="MessageBox"/>.
		/// </summary>
		/// <param name="value">The screen to show the <see cref="MessageBox"/> on.</param>
		/// <param name="formatText">The formattext to display on the <see cref="MessageBox"/>.</param>
		/// <param name="args">The arguments of the formating-text.</param>
		public static void ShowMessage(this IScreenObject value, string formatText, params object[] args)
		{
			value.ShowMessageBox(String.Format(formatText, args), "Hinweis", Microsoft.LightSwitch.Presentation.Extensions.MessageBoxOption.Ok);
		}

		/// <summary>
		/// Dislays a <see cref="MessageBox"/> indication an exception.
		/// </summary>
		/// <param name="value">The screen to show the <see cref="MessageBox"/> on.</param>
		/// <param name="ex">The exception to display.</param>
		public static void ShowError(this IScreenObject value, Exception ex)
		{
#if DEBUG
			Debug.WriteLine(ex.ToString());
#endif
			value.ShowMessageBox(ex.ToString(), "Fehler", Microsoft.LightSwitch.Presentation.Extensions.MessageBoxOption.Ok);
		}

		/// <summary>
		/// Retrieves a full path of a temporarary file with the specified fileextension.
		/// This method ensures a unique non-existing filename.
		/// </summary>
		/// <param name="extension">The extension of the file.</param>
		/// <returns>The full path of the file.</returns>
		public static string GetFreeTempFilename(string extension)
		{
			byte[] num = new byte[4];
			string result = null;
			Random random = new Random();
			extension = "." + extension.Trim('.');
			do
			{
				random.NextBytes(num);
				try
				{
					result = Path.Combine(Path.GetTempPath(), BitConverter.ToUInt32(num, 0).ToString() + extension);
				}
				catch
				{
					continue;
				}
			} while (File.Exists(result));
			return result;
		}

		/// <summary>
		/// This method displays an <see cref="InputBox"/> and returns - after some validation - a proper filename, or <code>null</code> if the user aborts the operatrion.
		/// </summary>
		/// <param name="value">The screen to show the <see cref="InputBox"/> on.</param>
		/// <param name="text">The description of the  <see cref="InputBox"/>.</param>
		/// <param name="title">The title of the  <see cref="InputBox"/>.</param>
		/// <param name="defaultExtension">The default extension to add to the filename.<see cref="InputBox"/>.</param>
		/// <param name="initialFilename">The initial filename showing up.</param>
		/// <returns>The filename the user had entered; or <code>null</code> if he had aborted the operation.</returns>
		public static string FilenameInputBox(this IScreenObject value, string text, string title, string defaultExtension = null, string initialFilename = null)
		{
			string input = (initialFilename ?? "").Trim();
			string ext = (defaultExtension ?? "").Trim().TrimStart('.').Trim();

			while (true)
			{
				input = (value.ShowInputBox(text, title, input) ?? "").Trim().TrimStart('\\');

				if (String.IsNullOrEmpty(input))
				{
					value.ShowMessageBox("Ungültige Eingabe.");
					return null;
				}

				// Check if valid UNC-path
				if (input.IndexOfAny(Path.GetInvalidPathChars()) != -1)
					continue;

				break;
			}

			if ((input[1] == ':') && (input[2] == '\\') && (input.Length > 3))
				// Absolute Path
				input = Path.GetFullPath(input);
			else
				// Relative Path
				input = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), input));

			// Add extension, if we had been passed a defaut extension
			if (!String.IsNullOrEmpty(ext))
				return Path.HasExtension(input) && (StringComparer.InvariantCultureIgnoreCase.Compare(Path.GetExtension(input), ext) == 0) ? Path.ChangeExtension(input, ext) : input + '.' + ext;
			else
				return input;
		}

		/// <summary>
		/// Starts an process using the passed parameters.
		/// </summary>
		/// <param name="filename">The filename to execute.</param>
		/// <param name="arguments">The arguments to pass to the process. This parameter is optional.</param>
		/// <param name="workingDirectory">The workingdirectory of the process. This parameter is optional.</param>
		/// <param name="operation">The operation to be performed. This parameter is optional. By default, <see cref="ProcessVerb.Open"/> will be used.</param>
		/// <param name="visibility">A <see cref="ProcessVisibility"/>-value to determine how the new process will be shown. This parameter is optional. By default, <see cref="ProcessVisibility.Normal"/> will be used.</param>
		/// <param name="suppressErrors">A value determining if exception will be supressed or not.</param>
		/// <returns><code>true</code> if the operation succeeds, otherwise <code>false</code>.</returns>
		public static bool ShellExecute(string filename, string arguments = null, string workingDirectory = null, ProcessVerb operation = ProcessVerb.Open, ProcessVisibility visibility = ProcessVisibility.Normal, bool suppressErrors = true)
		{
			if (!AutomationFactory.IsAvailable)
				throw new Exception("Automation unavailable due to insufficient rights.");

			if (filename == null)
				throw new ArgumentNullException("commandline");

			if (String.IsNullOrWhiteSpace(filename))
				throw new ArgumentException("Invalid commandline.", "commandline");

			var args = arguments ?? "";
			var dir = workingDirectory ?? "";
			var verb = operation.ToString().ToLowerInvariant();
			int mode = (int)visibility;

			dynamic shell = null;
			try
			{
				using (shell = AutomationFactory.CreateObject("Shell.Application"))
				{
					shell.ShellExecute(filename, args, dir, verb, mode);
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				Debug.WriteLine(ex.ToString());
#endif
				if (suppressErrors)
					return false;

				throw ex;
			}
			finally
			{
				shell = null;
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

			return true;
		}
	};
}
