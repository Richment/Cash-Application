namespace LightSwitchApplication
{
	using System;
	using System.IO;
	using System.Windows;
	using System.Windows.Controls;

	public partial class SelectFileWindow : ChildWindow
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SelectFileWindow), new PropertyMetadata(String.Empty));
		public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(SelectFileWindow), new PropertyMetadata("Alle Dateien (*.*)|*.*"));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		public string Filter
		{
			get { return (string)GetValue(FilterProperty); }
			set { SetValue(FilterProperty, value); }
		}
		public FileStream DocumentStream
		{
			get
			{
				return documentStream;
			}
			set
			{
				documentStream = value;
			}
		}
		public string SafeFileName
		{
			get
			{
				return safeFileName;
			}
			set
			{
				safeFileName = value;
			}
		}


		private FileStream documentStream;
		private string safeFileName;

		public SelectFileWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// OK Button
		/// </summary>
		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		/// <summary>
		/// Cancel button
		/// </summary>
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		/// <summary>
		/// Browse button
		/// </summary>
		private void BrowseButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			
			openFileDialog.Filter = Filter;

			if (openFileDialog.ShowDialog().GetValueOrDefault(false))
			{
				this.FileTextBox.Text = openFileDialog.File.Name;
				this.safeFileName = openFileDialog.File.Name;
				this.FileTextBox.IsReadOnly = true;
				FileStream myStream = openFileDialog.File.OpenRead();
				this.documentStream = myStream;
			}
		}
	};
}