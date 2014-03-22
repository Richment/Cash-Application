using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.LightSwitch.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;

namespace LightSwitchApplication
{
	public class ModalWrapper
	{
		public string WindowName
		{
			get;
			private set;
		}
		public string HeaderName
		{
			get;
			private set;
		}
		public Action ProceedMethod
		{
			get
			{
				return proceedMethod;
			}
			set
			{
				proceedMethod = value ?? new Action(() => { });
			}
		}
		public Action CancelMethod
		{
			get
			{
				return cancelMethod;
			}
			set
			{
				cancelMethod = value ?? new Action(() => { });
			}
		}
		public bool? Result
		{
			get;
			private set;
		}
		public object Tag 
		{
			get; 
			set; 
		}

		private readonly IScreenObject screen;
		private readonly string header;
		private Action proceedMethod, cancelMethod;

		public ModalWrapper(IScreenObject screen, string windowName, string headerName = null, string headerText = null)
		{
			this.screen = screen;
			this.WindowName = windowName;
			this.HeaderName = headerName;
			this.header = headerText ?? "";
			this.proceedMethod = () => { };
			this.cancelMethod = () => { };
			this.Result = null;
			this.Tag = null;
		}

		public void Show()
		{
			OpenModalWindow();
		}

		public void Close()
		{
			CloseModalWindow();
		}

		private void OpenModalWindow()
		{
			screen.OpenModalWindow(WindowName);
			screen.FindControl(WindowName).ControlAvailable += new EventHandler<ControlAvailableEventArgs>(OnModalAvailable);
		}

		private void CloseModalWindow()
		{
			screen.CloseModalWindow(WindowName);
			screen.FindControl(WindowName).IsVisible = false;
		}

		private void OnModalAvailable(object sender, ControlAvailableEventArgs e)
		{
			ChildWindow win = e.Control as ChildWindow;
			screen.FindControl(WindowName).ControlAvailable -= OnModalAvailable;
			if (!string.IsNullOrEmpty(HeaderName))
				screen.FindControl(HeaderName).SetProperty("Text", header);
			win.Closed += new EventHandler(OnModalClosed);
		}

		private void OnModalClosed(object sender, EventArgs e)
		{
			ChildWindow win = sender as ChildWindow;
			win.Closed -= OnModalClosed;

			this.Result = win.DialogResult;

			if (!Result.HasValue)
			{
				cancelMethod();
			}
			else if (Result.GetValueOrDefault(false))
			{
				proceedMethod();
			}

			this.CloseModalWindow();
		}
	};
}
