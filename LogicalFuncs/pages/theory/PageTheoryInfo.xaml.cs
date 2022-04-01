using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace LogicalFuncs.pages.theory
{
    public partial class PageTheoryInfo : System.Windows.Controls.Page
    {
        private string docFileName { get; set; }
        private static XpsDocument xpsDocument { get; set; }
        public PageTheoryInfo(string docFileName)
        {
            InitializeComponent();

            this.docFileName = docFileName;
            LoadDocFile(docFileName);
        }


        private async void LoadDocFile(string docFileName)
        {
            string fullName = Environment.CurrentDirectory + docFileName;
            if (File.Exists(fullName))
            {
                if (!File.Exists(fullName.Replace(".docx", ".xps")))
                {
                    string newXPSDocumentName = String.Concat(System.IO.Path.GetDirectoryName(fullName), "\\", System.IO.Path.GetFileNameWithoutExtension(docFileName), ".xps");
                    XpsDocument document = null;
                    while (document == null)
                    {
                        try
                        {
                            document = ConvertWordDocToXPSDoc(fullName, newXPSDocumentName).Result;
                        }
                        catch (Exception)
                        { continue; }
                    }
                    documentViewer.Document = document.GetFixedDocumentSequence();
                    xpsDocument.Close();
                }
                else
                {
                    documentViewer.Document = new XpsDocument(fullName.Replace(".docx", ".xps"), System.IO.FileAccess.Read).GetFixedDocumentSequence();
                }
            }
            else
            {
                MessageBox.Show($"Файл {docFileName.Split('\\')[4]} не был найден по указанному пути: {fullName}! Проверьте наличие файла по указанному пути.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async static Task<XpsDocument> ConvertWordDocToXPSDoc(string wordDocName, string xpsDocName)
        {
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            wordApplication.Documents.Add(wordDocName);
            Document doc = wordApplication.ActiveDocument;
            try
            {
                doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();
                XpsDocument xpsDoc = new XpsDocument(xpsDocName, System.IO.FileAccess.Read);
                xpsDocument = xpsDoc;
                return xpsDoc;
            }
            catch (Exception exp)
            {
                string str = exp.Message;
            }
            return null;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.PageContener.GoBack();
        }
    }
}
