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
using System.Windows;
namespace LightSwitchApplication
{
   /* public partial class Application
    {
        //Setup: Please place all the spreadsheets and documents located in the root 
        // of the sample ZIP into your "My Documents" folder to run this sample.
        public string MyDocsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string _WordExportDocument;
        string _WordReportDocument;

        public string WordExportDocument
        {
            get
            {
                if (_WordExportDocument == null) 
                { _WordExportDocument = MyDocsLocation + "\\RechnungExport2.docx"; }

                if (!File.Exists(_WordExportDocument))
                {
                    Dispatchers.Main.Invoke(() => MessageBox.Show("Please locate the Word export document to run this sample"));
                    _WordExportDocument = this.GetFile("Word Files (*.docx)|*.docx");
                }
                return _WordExportDocument;
            }
        }

        public string WordReportDocument
        {
            get
            {
                if (_WordReportDocument == null)
                { _WordReportDocument = MyDocsLocation + "\\BookReport.docx"; }

                if (!File.Exists(_WordReportDocument))
                {
                    Dispatchers.Main.Invoke(() => MessageBox.Show("Please locate the Word report document to run this sample: i.e. BookReport.docx"));
                    _WordReportDocument = this.GetFile("Word Files (*.docx)|*.docx");
                }
                return _WordReportDocument;
            }
        }

        private string GetFile(string filter)
        {
            System.IO.FileInfo file = null;
            Dispatchers.Main.Invoke(() =>
            {
                System.Windows.Controls.OpenFileDialog dlg = new System.Windows.Controls.OpenFileDialog();
                dlg.Filter = filter;

                if (dlg.ShowDialog() == true)
                {
                    file = dlg.File;
                }
            });

            try
            {
                if (((file != null)))
                {
                    return file.FullName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Dispatchers.Main.Invoke(() => MessageBox.Show(ex.ToString()));
                return "";
            }
        }
        
        partial void CreateNewRechnung_CanRun(ref bool result)
        {
            // Ergebnis auf den gewünschten Feldwert festlegen

        }

        partial void ListDetail_Rechnungen_CanRun(ref bool result)
        {
            // Ergebnis auf den gewünschten Feldwert festlegen

        }

        partial void KundeSuchen_CanRun(ref bool result)
        {
            // Ergebnis auf den gewünschten Feldwert festlegen

        }
    }
    */
}
