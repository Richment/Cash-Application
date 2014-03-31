namespace LightSwitchApplication
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.InteropServices.Automation;
	using System.Threading;

	/// <summary>
	/// This class reprents a Worddocument. This class can not be inherited.
	/// </summary>
	public sealed class WordDocument : IDisposable
	{
		private readonly object syncLock = new Object();
		private dynamic app;

		/// <summary>
		/// Gets the actual filename of this document.
		/// </summary>
		public string Filename
		{
			get;
			private set;
		}

		/// <summary>
		/// This Constructor instanciates a new <see cref="WordDocument"/>-class.
		/// </summary>
		/// <param name="filename">The filename of the document to load.</param>
		/// <param name="action">The <see cref="Action"/> to perform when the document is loaded. This parameter is optional. </param>
		public WordDocument(string filename, Action<dynamic> action = null)
		{
			if (!AutomationFactory.IsAvailable)
				throw new Exception("Automation unavailable due to insufficient rights.");

			if (filename == null)
				throw new ArgumentNullException("filename");

			if (!File.Exists(filename))
				throw new FileNotFoundException(String.Format("'{0}' does not exist.", filename));

			dynamic wordDocument = null;
			try
			{
				app = AutomationFactory.CreateObject("Word.Application");
				wordDocument = app.Documents.Open(filename);
				wordDocument.Activate();

				if (action != null)
					action(app);

				Filename = filename;
				app.Visible = false;
			}
			catch
			{
				SafeDispose(ref app);
				throw;
			}
			finally
			{
				wordDocument = null;
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		~WordDocument()
		{
			lock (syncLock)
			{
				Dispose();
			}
			GC.ReRegisterForFinalize(this);
		}

		/// <summary>
		/// Saves the document; optionally to a specific filename.
		/// </summary>
		/// <param name="filename">The filename the document should be saved to. This parameter is optional.</param>
		/// <returns><code>true</code> if the operation succeeds, otherwise <code>false</code>.</returns>
		public bool Save(string filename = null)
		{
			lock (syncLock)
			{
				try
				{
					return SaveAs(filename ?? Filename);
				}
				catch (Exception ex)
				{
					throw new Exception("Saving document failed.", ex);
				}
			}
		}

		/// <summary>
		/// Saves the document to a specific filename.
		/// </summary>
		/// <param name="filename">The filename the document should be saved to.</param>
		/// <returns><code>true</code> if the operation succeeds, otherwise <code>false</code>.</returns>
		public bool SaveAs(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");

			if (app == null)
				throw new ObjectDisposedException("The document has been already disposed.");

			lock (syncLock)
			{
				try
				{
					app.ActiveDocument.SaveAs(Path.GetFullPath(filename));
				}
#if DEBUG
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
#else
				catch
				{
#endif
					return false;
				}

				Filename = Path.GetFullPath(filename);
			}
			return true;
		}

		/// <summary>
		/// Prints the document.
		/// </summary>
		/// <returns><code>true</code> if the operation succeeds, otherwise <code>false</code>.</returns>
		public bool Print()
		{
			if (app == null)
				throw new ObjectDisposedException("The document has been already disposed.");

			lock (syncLock)
			{
				try
				{
					app.ActiveDocument.PrintOut();
					Thread.Sleep(1000);
				}
#if DEBUG
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
#else
				catch
				{
					return false;
#endif

				}
			}
			return true;
		}


		// Klappt noch nicht:
		/*public bool View()
		{
			if (app == null)
				throw new ObjectDisposedException("The document has been already disposed.");

			string filename = Path.Combine(Path.GetTempPath(), DateTime.Now.ToFileTime().ToString() + '.' + Path.GetExtension(Filename));
			try
			{
				app.ActiveDocument.SaveAs(Path.GetFullPath(filename));

				using (dynamic cmd = AutomationFactory.CreateObject("WScript.Shell"))
				{
					cmd.Run(filename, 1, true);
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				Debug.WriteLine(ex.ToString());
#endif
				return false;
			}
			return true;
		}*/

		/// <summary>
		/// Closes this document and releases all managed and unmanged resources assigned to this class.
		/// </summary>
		/// <param name="saveOnExit">If <code>true</code>, the document will be saved before it closes.</param>
		/// <returns><code>true</code> if the operation succeeds, otherwise <code>false</code>.</returns>
		public bool Close(bool saveOnExit = false)
		{
			if (app == null)
				throw new ObjectDisposedException("The document has been already disposed.");

			lock (syncLock)
			{
				try
				{
					SafeDispose(ref app, saveOnExit);
				}
#if DEBUG
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
#else
				catch
				{
#endif
					return false;
				}
			}
			return true;
		}

		private static void SafeDispose(ref dynamic app, bool saveOnExit = false)
		{
			if (app != null)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				try
				{
					app.Quit(saveOnExit ? -1 : 0);
				}
#if DEBUG
				catch (Exception ex)
				{
					Debug.WriteLine("Error while disposing COM-Object: " + ex.ToString());
#else
				catch
				{
#endif
				}
				finally
				{
					app = null;
					GC.Collect();
					GC.WaitForPendingFinalizers();
				}
			}
		}

		#region IDisposable Member

		/// <summary>
		/// Releases all managed and unmanged resources assigned to this class.
		/// </summary>
		public void Dispose()
		{
			lock (syncLock)
			{
				if (app != null)
				{
					GC.SuppressFinalize(this);
					SafeDispose(ref app);
				}
			}
		}

		#endregion
	};
}
